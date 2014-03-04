namespace Toptix.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Toptix.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<Toptix.Models.TCRMDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Toptix.Models.TCRMDBContext context)
        {
           // context.Products.AddOrUpdate(i => i.Name,
           //     new Product
           //     {
           //         Name = "bSRO",
           //         AppEnumID = 
           //         
                    
           //     },
           //     new Product
           //     {
           //         Name = "aSRO",
           //         Version = "4"
           //     },
           //     new Product
           //     {
           //         Name = "kSRO",
           //         Version = "4"
           //     }

           //);

            context.ActivityTypes.AddOrUpdate(i => i.Description,
                new ActivityType
                {
                    Description = "Sports"
                },
                new ActivityType
                {
                    Description = "Museum"
                },
             
                new ActivityType
                {
                    Description = "Cinema"
                },
                new ActivityType
                {
                    Description = "Theatre"
                },
                new ActivityType
                {
                    Description = "Non-Profit"
                }
             );


            context.EnumCalculationTypes.AddOrUpdate(i => i.Description,
               new EnumCalculationType
               {
                   Description = "Nominal"
               },
               new EnumCalculationType
               {
                   Description = "Percentage"
               }

            );

            context.EnumChargeFrequencies.AddOrUpdate(i => i.Description,
               new EnumChargeFrequency
               {
                   Description = "Monthly"
               },
               new EnumChargeFrequency
               {
                   Description = "Quarterly"
               },
               new EnumChargeFrequency
               {
                   Description = "Semi-Annual"
               },
               new EnumChargeFrequency
               {
                   Description = "Annual"
               }

            );

            context.ProductTypes.AddOrUpdate(i => i.Description,
            new ProductType
            {
                Description = "Lisence"
            },
            new ProductType
            {
                Description = "Temp Lisence"
            },

            new ProductType
            {
                Description = "Support"
            },
            new ProductType
            {
                Description = "Hosting"
            },
            new ProductType
            {
                Description = "HardWare"
            },
            new ProductType
            {
                Description = "Proffesional Services"
            }
         );




            //context.AppEnums.AddOrUpdate(i=>i.Description,
            //    new AppEnum
            //    {
            //        Description = "Nominal",
            //        Type = "CalculationType"
            //    },
            //    new AppEnum
            //    {
            //        Description = "Precentage",
            //        Type = "CalculationType"
            //    },
            //    new AppEnum
            //    {
            //        Description = "Sale",
            //        Type = "ActionType"
            //    },
            //    new AppEnum
            //    {
            //        Description = "Return",
            //        Type = "ActionType"
            //    },
            //    new AppEnum
            //    {
            //        Description = "Sports",
            //        Type = "ActivityType"
            //    },
            //    new AppEnum
            //    {
            //        Description = "Museum",
            //        Type = "ActivityType"
            //    },
            //    new AppEnum
            //    {
            //        Description = "Theatre",
            //        Type = "ActivityType"
            //    },
            //    new AppEnum
            //    {
            //        Description = "Cinema",
            //        Type = "ActivityType"
            //    },
            //    new AppEnum
            //    {
            //        Description = "Non-Profit",
            //        Type = "ActivityType"
            //    },
            //    new AppEnum
            //    {
            //        Description = "Monthly",
            //        Type = "ChargeFrequency"
            //    },
            //    new AppEnum
            //    {
            //        Description = "Quarterly",
            //        Type = "ChargeFrequency"
            //    },
            //    new AppEnum
            //    {
            //        Description = "Semi-Anuual",
            //        Type = "ChargeFrequency"
            //    },
            //    new AppEnum
            //    {
            //        Description = "Annual",
            //        Type = "ChargeFrequency"
            //    },
            //    new AppEnum
            //    {
            //        Description = "Temp Lisence",
            //        Type = "ProductType"
            //    },
            //    new AppEnum
            //    {
            //        Description = "Lisence",
            //        Type = "ProductType"
            //    },
            //    new AppEnum
            //    {
            //        Description = "Support",
            //        Type = "ProductType"
            //    },
            //    new AppEnum
            //    {
            //        Description = "Hardware",
            //        Type = "ProductType"
            //    },
            //    new AppEnum
            //    {
            //        Description = "Proffesional Services",
            //        Type = "ProductType"
            //    }

            //    );

        }
    }
}
