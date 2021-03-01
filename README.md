# NAdmin

Easy to use boilerplate for admin panel written in .NET 5.

## Setup

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddNAdmin(conf =>
    {
        conf.UseSqlServerConnection(Configuration.GetConnectionString("AdminPanelConnectionString"));
    });
    
    services.AddControllersWithViews();
}
```

```c#
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    // ...

    app.UseNAdmin();

    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
    });
}
```