using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions.CompanyExceptions
{
    public class CompanyCollectionBadRequest:BadRequestException
    {
        public CompanyCollectionBadRequest():base("Company collection sent from a client is null.")
        {

        }
    }
}
