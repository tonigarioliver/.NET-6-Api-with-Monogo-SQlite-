using LearnApi.Models;

namespace LearnApi.Servies
{
    public interface ICustomerService
    {
        public Task<List<CustomerModel>> GetAll();
        public Task<CustomerModel> AddCustomer(CustomerModel book);
        public Task<CustomerModel> GetbyName(string customerName);
        public Task<ApiResponse> Remove(string customerName);

        public Task<ApiResponse> Update(CustomerModel customer, string customerName);
    }
}
