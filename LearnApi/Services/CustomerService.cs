using AutoMapper;
using LearnApi.COR;
using LearnApi.Data;
using LearnApi.Entity;
using LearnApi.Models;
using Microsoft.Extensions.Logging;

namespace LearnApi.Servies
{
    public class CustomerService : ICustomerService
    {
        public IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<ICustomerService> logger;

        public CustomerService(IUnitOfWork unitOfWork,IMapper mapper, ILogger<ICustomerService> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }


        public async Task<CustomerModel> AddCustomer(CustomerModel customer)
        {
            if (customer == null)
            {
                return null;
            }
            if (await unitOfWork.CustomerRepository.GetAsync(u => u.Name.ToLower() == customer.Name.ToLower()) != null)
            {
                return null;
            }
            Customer newCustomer = mapper.Map<Customer>(customer);
            await unitOfWork.CustomerRepository.CreateAsync(newCustomer);
            await unitOfWork.CompleteAsync();
            return customer;
        }


        public async Task<List<CustomerModel>> GetAll()
        {
            var customers = await unitOfWork.CustomerRepository.GetAllAsync();
            return mapper.Map<List<CustomerModel>>(customers);
        }

        public async Task<CustomerModel> GetbyName(string customerName)
        {
            var customer = await unitOfWork.CustomerRepository.GetAsync(customer => customer.Name == customerName);
            if (customer == null)
            {
                return null;
            }
            return mapper.Map<CustomerModel>(customer);
        }

        public async Task<ApiResponse> Remove(string customerName)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var customer = await unitOfWork.CustomerRepository.GetAsync(customer => customer.Name == customerName);
                if (customer != null)
                {
                    await unitOfWork.CustomerRepository.Delete(customer);
                    await unitOfWork.CompleteAsync();
                    response.StatusCode = StatusCodes.Status200OK;
                    response.Result = customerName;
                }
                else
                {
                    response.StatusCode = StatusCodes.Status404NotFound;
                    response.Errormessage = "Data not found";
                }

            }
            catch (Exception ex)
            {
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.Errormessage = ex.Message;
            }
            return response;
        }

        public async Task<ApiResponse> Update(CustomerModel customer, string customerName)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                // Buscar el cliente existente por nombre
                var existingCustomer = await unitOfWork.CustomerRepository.GetAsync(customer => customer.Name == customerName);

                if (existingCustomer != null)
                {
                    // Usar AutoMapper para mapear las propiedades del modelo actualizado al cliente existente
                    mapper.Map(customer, existingCustomer);
                    await unitOfWork.CustomerRepository.Update(existingCustomer);
                    // Guardar los cambios en la base de datos
                    await unitOfWork.CompleteAsync();
                    response.StatusCode = StatusCodes.Status200OK;
                    response.Result = customerName;
                }
                else
                {
                    response.StatusCode = StatusCodes.Status404NotFound;
                    response.Errormessage = "Data not found";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.Errormessage = ex.Message;
            }
            return response;
        }
    }
}
