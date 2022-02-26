using PeopleData.Business.Services;
using PeopleData.Business.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PeopleData.ConsoleApp
{
    public class App
    {
        private IPersonService _personService;

        public App(IPersonService personService){

            _personService = personService;
        }

        private async Task Menu() {

            var people = await _personService.GetPeople();
            Console.WriteLine(people.Value.Count);
        }

        public async Task Start() {
            Console.WriteLine("Starting Application...");
            await Menu();
        }

        
    }
}
