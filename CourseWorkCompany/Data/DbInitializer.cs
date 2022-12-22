using CourseWorkCompany.Models;
using System.Collections.Specialized;

namespace CourseWorkCompany.Data
{
    public static class DbInitializer
    {
        public static void Initialize(Context context)
        {
            context.Database.EnsureCreated();
            if (context.ProductionTypes.Any())
            {
                return;
            }
            List<string> Name = new List<string> { "Игорь", "Артемий", "Максим", "Райан", "Николай", "Кристиан", "Патрик", "Аратаки" };
            List<string> Surname = new List<string> { "Стрелков", "Бейтман", "Гофман", "Рыбников", "Рейген", "Кристиан", "Патрик" };

            for (int id = 1; id <= 50; id++)
            {
                string productType = "Тип продукции" + new Random().Next(100).ToString();
                context.ProductionTypes.Add(new ProductionType { Name = productType });
            }
            for (int id = 1; id <= 150; id++)
            {
                string name = Name[new Random().Next(Name.Count - 1)] + " " + Surname[new Random().Next(Name.Count - 1)];
                string company = "Предприятие" + new Random().Next(100).ToString();
                string activity = "Активность" + new Random().Next(100).ToString();
                string typeActivity = "Тип активности" + new Random().Next(100).ToString();
                context.Companies.Add(new Company { FIO = name, KindOfActivity = activity, Name = company, TypeOwnership = typeActivity });
            }
            context.SaveChanges();

            for (int id = 1; id <= 150; id++)
            {
                string name = "Продукт" + new Random().Next(100).ToString();
                string characteristic = "Характеристика" + new Random().Next(100).ToString();
                string measurementUnit = "Единица измерения" + new Random().Next(100).ToString();
                int typeProduct = new Random().Next(1, context.ProductionTypes.Count());
                context.Products.Add(new Product { Name = name, Characteristic = characteristic, MeasurementUnit = measurementUnit, ProductionTypeId = typeProduct });
            }
            context.SaveChanges();

            for (int id = 1; id <= 150; id++)
            {
                int company = new Random().Next(1, context.Companies.Count());
                int product = new Random().Next(1, context.ProductionTypes.Count());
                int plannedImplementationVolume = new Random().Next(1, 100);
                int actualImplementationVolume = new Random().Next(1, 100);
                DateTime quarterInfo = DateTime.Now.AddDays(new Random().Next(1000));
                DateTime yearInfo = DateTime.Now.AddDays(new Random().Next(1000));
                context.ProductSalesPlans.Add(new ProductSalesPlan
                {
                    CompanyId = company,
                    ProductionTypeId = product,
                    ActualImplementationVolume = actualImplementationVolume,
                    YearInfo = yearInfo,
                    QuarterInfo = quarterInfo,
                    PlannedImplementationVolume = plannedImplementationVolume
                });
            }
            for (int id = 1; id <= 150; id++)
            {
                int company = new Random().Next(1, context.Companies.Count());
                int product = new Random().Next(1, context.ProductionTypes.Count());
                int plannedImplementationVolume = new Random().Next(1, 100);
                int actualImplementationVolume = new Random().Next(1, 100);
                DateTime quarterInfo = DateTime.Now.AddDays(new Random().Next(1000));
                DateTime yearInfo = DateTime.Now.AddDays(new Random().Next(1000));
                context.ProductReleasePlans.Add(new ProductReleasePlan
                {
                    CompanyId = company,
                    ProductionTypeId = product,
                    ActualOutputVolume = actualImplementationVolume,
                    YearInfo = yearInfo,
                    QuarterInfo = quarterInfo,
                    PlannedOutputVolume = plannedImplementationVolume
                });
            }
            context.SaveChanges();
        }
    }
}