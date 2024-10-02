using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopApp.BLL.Abstract;
using ShopApp.BLL.Concrete;
using ShopApp.DAL.Abstract;
using ShopApp.DAL.Concrete.EfCore;
using ShopApp.DAL.Concrete.Memory;
using ShopApp.WebUI.Identity;
using ShopApp.WebUI.Middlewares;

namespace ShopApp.WebUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnect")));


            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
                .AddDefaultTokenProviders();


            var usermanager = builder.Services.BuildServiceProvider().GetService<UserManager<ApplicationUser>>();
            var rolemanager = builder.Services.BuildServiceProvider().GetService<RoleManager<IdentityRole>>();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                //password

                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 6;

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;

            });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/account/accessdenied";
                options.LoginPath = "/Account/login";
                options.LogoutPath = "/Account/logout";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.SlidingExpiration = true;
                options.Cookie = new CookieBuilder()
                {
                    HttpOnly = true,
                    Name = "ShopApp.Security.Cookie",
                    SameSite = SameSiteMode.Strict
                };
            });


            //Dependency Injection : Baðýmlýlýk Yönetimi
            builder.Services.AddScoped<IProductDal, EfCoreProductDal>();
            builder.Services.AddScoped<IProductService, ProductManager>();
            builder.Services.AddScoped<ICategoryDal, EfCoreCategoryDal>();
            builder.Services.AddScoped<ICategoryService, CategoryManager>();
            builder.Services.AddScoped<ICommentDal, EfCoreCommentDal>();
            builder.Services.AddScoped<ICommentService, CommentManager>();
            builder.Services.AddScoped<ICartDal, EfCoreCartDal>();
            builder.Services.AddScoped<ICartService, CartManager>();
            builder.Services.AddScoped<IOrderService, OrderManager > ();
            builder.Services.AddScoped<IOrderDal, EfCoreOrderDal > ();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            SeedDatabase.Seed();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.CustomStaticFiles();
            app.UseAuthentication();
            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");


            app.MapControllerRoute(
                name: "products",
                pattern: "products/{category?}",
                defaults: new { controller = "Shop", action = "List" }
                );

            app.MapControllerRoute(
                name: "adminProducts",
                pattern: "admin/products",
                defaults: new { controller = "Admin", action = "ProductList" }
                );

            app.MapControllerRoute(
             name: "adminProducts",
             pattern: "admin/products/{id?}",
             defaults: new { controller = "Admin", action = "EditProduct" }
             );

            app.MapControllerRoute(
                name: "adminCategories",
                pattern: "admin/categories",
                defaults: new { controller = "Admin", action = "CategoryList" }
                );

            app.MapControllerRoute(
                name: "adminCategories",
                pattern: "admin/categories/{id?}",
                defaults: new { controller = "Admin", action = "EditCategory" }
                );

            app.MapControllerRoute(
              name: "cart",
              pattern: "cart",
              defaults: new { controller = "Cart", action = "Index" }
              );

            app.MapControllerRoute(
              name: "checkout",
              pattern: "checkout",
              defaults: new { controller = "Cart", action = "checkout" }
              );

            app.MapControllerRoute(
             name: "orders",
             pattern: "orders",
             defaults: new { controller = "Cart", action = "GetOrders" }
             );

            SeedIdentity.Seed(usermanager, rolemanager).Wait();

            app.Run();
        }
    }
}