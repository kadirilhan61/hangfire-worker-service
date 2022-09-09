
using Hangfire;
using Hangfire.Console;
using Hangfire.Dashboard.BasicAuthorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using WorkerService1;

CreateHostBuilder(args)
            .Build()
            .Run();

static IHostBuilder CreateHostBuilder(string[] args) =>
      Host.CreateDefaultBuilder(args)
          .ConfigureWebHostDefaults(builder =>
          {
              builder.Configure(app =>
              {
                  app.UseRouting();

                  app.UseHangfireDashboard();
                  app.UseEndpoints(endpoints =>
                  {
                      endpoints.MapHangfireDashboard("/hangfire", new DashboardOptions
                      {
                          Authorization =
                          new[]
                          { new BasicAuthAuthorizationFilter(new BasicAuthAuthorizationFilterOptions
                              {
                                        RequireSsl = false,
                                        SslRedirect = false,
                                        LoginCaseSensitive = true,
                                        Users = new []
                                        {
                                            new BasicAuthAuthorizationUser
                                            {
                                            Login = "admin",
                                            PasswordClear =  "test"
                                            }
                                        }
                              }
                          )}
                      }); ;
                  });
              });
          })
          .ConfigureServices((hostContext, services) =>
          {
              IConfiguration config = hostContext.Configuration;

              services.AddHangfire(conf =>
              {
                  conf.UseSqlServerStorage(config.GetConnectionString("HangfireConnection"));
                  conf.UseConsole();
              });
              services.AddHangfireServer();

              // your worker service
              services.AddHostedService<Worker>();
          });
