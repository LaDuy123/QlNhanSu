using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BTL.Data
{
    public class RefreshToken
    {
        [Key]
        public Guid Id { get; set; }
        public int AdminId { get; set; }
        [ForeignKey(nameof(AdminId))]
        [JsonIgnore]
        public Admin Admin { get; set; }

        public string Token { get; set; }
        public string JwtId { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime ExpiredAt { get; set; }
    }
}
