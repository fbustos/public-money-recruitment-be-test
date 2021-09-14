using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using VacationRental.Api.Handlers;
using VacationRental.Api.Handlers.BookingHandler;
using VacationRental.Api.Handlers.CalendarHandler;
using VacationRental.Api.Handlers.RentalHandler;
using VacationRental.Domain.Bookings;
using VacationRental.Domain.Rental;
using VacationRental.Infrastructure;

namespace VacationRental.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerGen(opts => opts.SwaggerDoc("v1", new Info { Title = "Vacation rental information", Version = "v1" }));

            services.AddSingleton<IRentalRepository>(new MemRentalRepository(new Dictionary<int, Rental>()));
            services.AddSingleton<IBookingRepository>(new MemBookingRepository(new Dictionary<int, Booking>()));

            services.AddScoped<GetRental>();
            services.AddScoped<CreateRental>();
            services.AddScoped<UpdateRental>();
            services.AddScoped<GetBooking>();
            services.AddScoped<CreateBooking>();
            services.AddScoped<GetCalendar>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(opts => opts.SwaggerEndpoint("/swagger/v1/swagger.json", "VacationRental v1"));
        }
    }
}
