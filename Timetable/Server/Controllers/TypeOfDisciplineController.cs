using System;
using System.Collections.Generic;
using System.Web.Http;
using Models;
using Server.Services;

namespace Server.Controllers
{
    public class TypeOfDisciplineController : ApiController
    {
        public List<TypeOfDiscipline> GetAllTypeOfDisciplines()
        {
            var query = ConnectionInstance.connection.Execute(Queries.selectTypeOfDisciplines);
            var result = new List<TypeOfDiscipline>();
            foreach (var element in query)
            {
                result.Add(new TypeOfDiscipline
                    {
                        Type_ID = Convert.ToInt32(element[0]),
                        Type_name = element[1]
                    }
                );
            }
            return result;
        }
    }
}
