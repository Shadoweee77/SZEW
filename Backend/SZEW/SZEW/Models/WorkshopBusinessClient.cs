﻿namespace SZEW.Models
{
    public class WorkshopBusinessClient : WorkshopClient
    {
        public required string BusinessName { get; set; }
        public required string NIP { get; set; }
    }
}
