using System;

namespace GTIWebAPI.Models.Clients
{
    public class ClientSignerDTO
    {
        public int Id { get; set; }

        public int? ClientId { get; set; }

        public string UkrNomCase { get; set; }

        public string UkrGenCase { get; set; }
        
        public string RusNomCase { get; set; }

        public string RusGenCase { get; set; }

        public string EngNomCase { get; set; }

        public string Remark { get; set; }

        public int? SignerPositionId { get; set; }

        public DateTime? DateBeg { get; set; }

        public DateTime? DateEnd { get; set; }

        public SignerPositionDTO SignerPosition { get; set; }
    }
}