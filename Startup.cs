using SolrNet;
using SolrIrApp.Models;
using SolrIrApp.Services;

public class Startup
{
    public IConfiguration Configuration { get; }
    public static IConfiguration StaticConfiguration { get; private set;}

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
        StaticConfiguration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {

        services.Configure<RouteOptions>(options =>
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = true;
            options.AppendTrailingSlash = true;
        });

        services.AddRazorPages();
        services.AddSolrNet<Book>(Configuration["SolrUrl"]);
        services.AddScoped<Indexer<Book>>();
        services.AddScoped<Searcher<Book>>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
        }

        app.UseStaticFiles();
        app.UseRouting();

        app.UseEndpoints(endpoints =>
            endpoints.MapRazorPages()
        );
    }
}
