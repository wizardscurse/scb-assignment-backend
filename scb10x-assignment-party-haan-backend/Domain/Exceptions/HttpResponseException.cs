using System;
namespace scb10x_assignment_party_haan_backend.Domain.Exceptions
{
    public class HttpResponseException : Exception
    {
        public int Status { get; set; } = 500;

        public object Value { get; set; }
    }
}
