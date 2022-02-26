using PeopleData.Business.Services;
using PeopleData.Business.Services.Implementation;
using PeopleData.Data.Models.Response;
using PeopleData.Data.Models.Response.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleData.ConsoleApp
{
    public class App
    {
        private IPersonService _personService;
        private  Dictionary<string, Person> PeopleMap{ get; set; }
        private  string SearchKeyInput { get; set; }
        private Nullable<PersonFilter> FilterChoice { get; set; }
        private bool AppIsActive { get; set; } = true;

        public App(IPersonService personService){

            _personService = personService;
            PeopleMap = new Dictionary<string, Person>();
        }

        private async Task CachePeopleData() {
            var response = await _personService.GetPeople();

            if (response.Response == ResponseCode.Ok &&  !response.IsResultEmpty() ) {

                var data = response.Result.Value;

                foreach (Person person in data) {

                    PeopleMap.Add(person.UserName, person);

                }

            }
            
        }

        private async Task <Person>  GetPerson(string userName)
        {
 

            Person person = null;

            if (PeopleMap.ContainsKey(userName))
            {
                  PeopleMap.TryGetValue(userName, out person);
            }

            if (person == null) {
                //call service
                var response= await _personService.FindPerson(userName);

                if (!response.IsResultEmpty()) {
                    person = response.Result;
                }
            }

            return person;

        }


        private async Task<bool>  UpdatePersonDetails(string userName)
        {

            var person =  await GetPerson(userName);
            bool updated = false;

            //person exist
            if (person != null)
            {
                updated = (await _personService.UpdatePerson(person)).Result;
                //update cache
                if (updated) PeopleMap.Add(person.UserName, person);
               

            }

            return updated;
        }

        private async Task<List<Person>> SearchPerson(PersonFilter filter, string value)
        {

            var people = new List<Person>();

            var response = await _personService.FilterPeople(filter, value);

            if (!response.IsResultEmpty() )
            {
                people = response.Result.Value.ToList();


            }

            return people;

        }


        private async Task<bool> DeletePerson(string userName)
        {

            var result = await GetPerson(userName);
            bool deleted = false;

            //person exist
            if (result != null)
            {
                deleted = (await _personService.DeletePerson(userName)).Result;
                //update cache
                if (deleted) PeopleMap.Remove(userName);

            }

            return deleted;
        }



        private  void Menu() {

            Console.WriteLine("---- People Data Application (Menu) ----");
            Console.WriteLine();
            Console.WriteLine("View All              : Press 1");
            Console.WriteLine("View Person details   : Press 2");
            Console.WriteLine("Search Person         : Press 3");
            Console.WriteLine("Delete Person         : Press 4");
            Console.WriteLine("Update Person         : Press 5");
            Console.WriteLine("Exit App              : Press 6");
            Console.WriteLine();
            Console.WriteLine("Enter choice: ");
        }

        public async Task Start()
        {
            Console.WriteLine("Loading...");
            await CachePeopleData();

            do
            {
                Menu();
                await HandleInputOutput();

            } while (AppIsActive);
        }

        private async Task HandleInputOutput() {



            try
            {
                var choice = Convert.ToInt32(Console.ReadLine());

                var message = "";

                switch(choice)
                {
                    case  1:
                         var people = PeopleMap.Values.ToList();
                         message = people.Count == 0 ? "no Data" : "";
                         DisplayData(people, message);
                        break;

                    case 2:
                        HandleInputValue();
                        var person = new Person();
                        if (!String.IsNullOrEmpty(SearchKeyInput)) {       
                            person = await GetPerson(SearchKeyInput);
                            message = person == null ? "Can't Find Person" : "";
                            ViewPersonDetails(person, message);
                        }
                    

                        break;

                    case 3:
                        HandleInputSearch();
                        people = new List<Person>();
                        if (!String.IsNullOrEmpty(SearchKeyInput) &&  FilterChoice.HasValue ) {
                            people = await SearchPerson(FilterChoice.Value, SearchKeyInput);
                            message = people.Count == 0 ? "no Data" : "";
                            DisplayData(people, message);
                        }
                      

                        break;
                    
                     case 4:
                        HandleInputValue();
                        if (!String.IsNullOrEmpty(SearchKeyInput)) {
                            var deleted = await DeletePerson(SearchKeyInput);
                            message = deleted ? "Deleted Successfully" : "";
                            Console.WriteLine(message);
                        }
                        break;
                    case 5:

                        HandleInputValue();
                        if (!String.IsNullOrEmpty(SearchKeyInput))
                        {
                            var updated = await UpdatePersonDetails(SearchKeyInput);
                            message = updated ? "Updated Successfully" : "";
                            Console.WriteLine(updated);
                        }
                        break;

                   case 6:
                        AppIsActive = false;
                        Exit();
                        break;
                    default:
                        Console.WriteLine("Wrong Input");
                        break;
                }

            }
            catch{
                Console.WriteLine("Wrong Input.. Please try again ");
            }
            

        }
        private void HandleInputValue(string keyName = "UserName") {
            try
            {
                Console.WriteLine($"Enter  {keyName}: ");
                SearchKeyInput = Console.ReadLine();
            }
            catch {
                SearchKeyInput = "";
            }
           
        }

        private void HandleInputSearch() {
            try
            {
                var allowedSearchfilters = Enum.GetValues(typeof(PersonFilter));

                int count = 1;
                int choiceLength = allowedSearchfilters.Length;

                Console.WriteLine("--- Search Filters ---");
                foreach (var filter in allowedSearchfilters)
                {
                    var pFilter = (PersonFilter)filter;
                    Console.Write($"Press {count} : {0} \n", pFilter.ToString());
                }

                Console.WriteLine("Enter choice: ");
                var choice = Console.Read();

                if (choice < choiceLength || choice > choiceLength) {
                    throw new Exception();
                }

                FilterChoice = (PersonFilter)choice;

                HandleInputValue("SearchKey");

            }
            catch
            {
                Console.WriteLine("Invalid Input");
                SearchKeyInput = "";
            }
        }

        private void DisplayData(List<Person> people, string message) {

            Console.WriteLine("Loading... \n");


            foreach (var person in people){
                Console.WriteLine(person);
               
            }

            PrintMessage(message);

            Console.WriteLine();
        }

        private void ViewPersonDetails (Person person, string message) {

            if (person != null) {
                Console.WriteLine(person);
                Console.WriteLine("Email: ", person.Emails);

                if (person.Features.Count > 0) {
                    Console.WriteLine("----Features----");
                    String.Join(",", person.Features);
                }

                Console.WriteLine("Favorite Feature: ", person.FavoriteFeature);

                if (person.AddressInfo.Count > 0)
                {
                    Console.WriteLine("----Addresses ----");
                    foreach (var address in person.AddressInfo) {
                        Console.Write("Address: {0}  City Name: {1} Country {2} \n", address.Address, address.City.Name, address.City.CountryRegion);
                    }
                }


                if (person.HomeAddress != null) {
                    Console.WriteLine("----Current Address ----");
                    var address = person.HomeAddress;
                    Console.Write("Address: {0}  City Name: {1} Country {2}", address.Address, address.City.Name, address.City.CountryRegion);
                }

            }





            PrintMessage(message);
        }

        private void PrintMessage(string message) {
           
            if (!String.IsNullOrEmpty(message))
            {
                Console.WriteLine();
                Console.WriteLine(message);
                Console.WriteLine();
            }
        }
        private void Exit()
        {
            Console.WriteLine("Application Exited...");
        }

    }
}
