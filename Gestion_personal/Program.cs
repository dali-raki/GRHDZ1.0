using Gestion_personal.Components;
using GrhDz.Apps.Avances;
using GrhDz.Apps.Dettes;
using Implementation.Services.Avance;
using Implementation.Services.Avances;
using Implementation.Services.Dashboard;
using Implementation.Services.Dashboards;
using Implementation.Services.Dettes;
using Implementation.Services.Employees;
using Implementation.Services.EmployeModel;
using Implementation.Services.EquipeEmploye;
using Implementation.Services.Equipes;
using Implementation.Services.Fonctions;
using Implementation.Services.LogsAction;
using Implementation.Services.LogsActions;
using Implementation.Services.PointageService;
using Implementation.Services.Post;
using Implementation.Services.Posts;
using Implementation.Services.Prime;
using Implementation.Services.Primes;
using Implementation.Services.ReadUSB;
using Implementation.Services.Remboursement;
using Implementation.Services.Salaire;
using Implementation.Services.SalaireBase;
using Implementation.Services.SalaireBases;
using Implementation.Services.Salaires;
using Implementation.Services.TypeDePaiment;
using Implementation.Services.Users;
using Infrastructures.Storages.AvancesStorages;
using Infrastructures.Storages.DashboardStorages;
using Infrastructures.Storages.DettesStorages;
using Infrastructures.Storages.EmployeesEquipesStorages;
using Infrastructures.Storages.EmployeesStorages;
using Infrastructures.Storages.EquipesStorages;
using Infrastructures.Storages.FonctionsStorages;
using Infrastructures.Storages.LogActionStorage;
using Infrastructures.Storages.PointagesStorages;
using Infrastructures.Storages.PostesStorages;
using Infrastructures.Storages.PrimesStorages;
using Infrastructures.Storages.RecordStorages;
using Infrastructures.Storages.RemboursementsStorages;
using Infrastructures.Storages.SalairesBaseStorages;
using Infrastructures.Storages.SalairesStorages;
using Infrastructures.Storages.TransferData;
using Infrastructures.Storages.TypeDePaimentStorages;
using Infrastructures.Storages.UserStorages;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Radzen;


var builder = WebApplication.CreateBuilder(args);

string connectionString =builder.Configuration.GetConnectionString("DBConnection");
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();
builder.Services.AddRazorComponents();
builder.Services.AddSingleton<IConfiguration>(provider =>
	new ConfigurationBuilder().AddJsonFile("appsettings.json").Build());
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped<IPointageStorage,PointageStorage>();
builder.Services.AddScoped<ISalaireStorage,SalaireStorage>();
builder.Services.AddScoped<ISalaireBaseStorage,SalaireBaseStorage>();
builder.Services.AddScoped<IEquipeStorage,EquipeStorage>();
builder.Services.AddScoped<IEmployeeEquipeStorage,EmployeeEquipeStorage> ();
builder.Services.AddScoped<IAvanceStorage,AvanceStorage>();
builder.Services.AddScoped<IDetteStorage,DetteStorage>();
builder.Services.AddScoped<IPosteStorage,PosteStorage>();

builder.Services.AddScoped<IDetteRestantStorage,DetteRestantStorage>();
builder.Services.AddScoped<IRemboursementStorage,RemboursementStorage>();
builder.Services.AddScoped<IPrimeStorage,PrimeStorage>();

builder.Services.AddScoped<IUserStorage,UserStorage>();
builder.Services.AddScoped<ITransferDataStorage, TransferDataStorage>();
builder.Services.AddScoped<ICheckInOutStorage, CheckInOutStorage>();

builder.Services.AddScoped<IDashboardStorage, DashboardStorage>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

builder.Services.AddScoped<IDetteService, DetteService>();

builder.Services.AddScoped<ITypeDePaiementStorage, TypeDePaiementStorage>();
builder.Services.AddScoped<ITypeDePaiementService, TypeDePaiementService>();

builder.Services.AddScoped<IEmployeService, EmployeService>();

builder.Services.AddScoped<IFonctionStorage, FonctionStorage>();
builder.Services.AddScoped<IFonctionService, FonctionService>();
builder.Services.AddScoped<IEmployeStorage, EmployeStorage>();

builder.Services.AddScoped<ITypeDePaiementStorage, TypeDePaiementStorage>();
builder.Services.AddScoped<ITypeDePaiementService, TypeDePaiementService>();


builder.Services.AddScoped<IPointageService, PointageService>();
builder.Services.AddScoped<ISalaireService, SalaireService>();
builder.Services.AddScoped<ISalaireBaseService, SalaireBaseService>();
builder.Services.AddScoped<IPDFService, PDFService>();
builder.Services.AddScoped<IEquipeService, EquipeService>();
builder.Services.AddScoped<IEmployeeEquipeService, EmployeeEquipeService>();
builder.Services.AddScoped<IPosteService,PosteService>();
builder.Services.AddScoped<IAvanceService, AvanceService>();
builder.Services.AddScoped<IDetteService, DetteService>();
builder.Services.AddScoped<IPdfService,PdfService>();
builder.Services.AddScoped<IDetteRestantService, DetteRestantService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPostGeneratePDF,PostGeneratePDF>();
builder.Services.AddSingleton<UserSessionStateService>();
builder.Services.AddScoped<IFileProcessingService, FileProcessingService>();
builder.Services.AddScoped<IRemboursementStorage, RemboursementStorage>();
builder.Services.AddScoped<IPrimeStorage, PrimeStorage>();
builder.Services.AddScoped<IRemboursementService,RemboursementService>();
builder.Services.AddScoped<IPrimeService,PrimeService>();
builder.Services.AddScoped<ILogsActionService, LogsActionService>();
builder.Services.AddControllers();
builder.Services.AddRadzenComponents();
builder.Services.AddScoped<TypeDePaiementStorage>();
builder.Services.AddScoped<FonctionStorage>();
builder.Services.AddScoped<PointageStorage>();
builder.Services.AddScoped<SalaireStorage>();
builder.Services.AddScoped<SalaireBaseStorage>();
builder.Services.AddScoped<PosteStorage>();
builder.Services.AddScoped<RemboursementStorage>();
builder.Services.AddScoped<PrimeStorage>();
builder.Services.AddScoped<LogActionStorage>();
// Repeat for other storages used directly in service constructors
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)

	.AddCookie(options =>
	{
		options.Cookie.Name = "Auth_Token";
		options.LoginPath = "/";
		options.Cookie.MaxAge = TimeSpan.FromDays(1);
		options.AccessDeniedPath = "/";
		options.LogoutPath = "/";
	});

builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection")));
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7276")
});
builder.Services.AddSingleton<UserSessionStateService>();
builder.Logging.SetMinimumLevel(LogLevel.Debug);
builder.Services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });
var app = builder.Build();
app.UseSession(); 
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
		app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();
app.Run();
