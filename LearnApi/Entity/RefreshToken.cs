using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LearnApi.Entity
{
    public class RefreshToken
    {
        [Key]
        [Column("tokenid")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Unicode(false)]
        public int Tokenid { get; set; }

        [Column("refreshtoken")]
        [Unicode(false)]
        public string? Refreshtoken { get; set; }
        [Column("ValidTo")]
        public DateTime ValidTo { get; set; }
        public User User { get; set; }
    }
    public class RefreshTokenBuilder
    {
        private int tokenid;
        private string refreshtoken;
        private DateTime validTo;
        private User user;

        public RefreshTokenBuilder WithTokenId(int tokenId)
        {
            this.tokenid = tokenId;
            return this;
        }

        public RefreshTokenBuilder WithRefreshToken(string refreshToken)
        {
            this.refreshtoken = refreshToken;
            return this;
        }

        public RefreshTokenBuilder WithValidTo(DateTime validTo)
        {
            this.validTo = validTo;
            return this;
        }

        public RefreshTokenBuilder WithUser(User user)
        {
            this.user = user;
            return this;
        }

        public RefreshToken Build()
        {
            return new RefreshToken
            {
                Tokenid = this.tokenid,
                Refreshtoken = this.refreshtoken,
                ValidTo = this.validTo,
                User = this.user
            };
        }
    }
 }
