using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace used_cars_arpas
{
    internal class Car
    {
        public string Name { get; set; }
        public string Model { get; set; }
        public string FuelType { get; set; }
        public int Mileage { get; set; }
        public int RegistrationDate { get; set; }
        public int Price { get; set; }
        public string Img { get; set; }

        public Car(string name, string model, string fuelType, int mileage, int registrationDate, int price, string img)
        {
            Name = name;
            Model = model;
            FuelType = fuelType;
            Mileage = mileage;
            RegistrationDate = registrationDate;
            Price = price;
            Img = img;
        }

        public override string ToString()
        {
            return $"Name: {Name}, Model: {Model}, Fuel Type: {FuelType}, Mileage: {Mileage}, Registration Date: {RegistrationDate}, Price: {Price}, Image source: {Img}";
        }
        public string YearNameModel
        {
            get { return $"{RegistrationDate} {Name} {Model}"; }
        }
    }
}
