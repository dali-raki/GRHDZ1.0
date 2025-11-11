using GrhDz.Apps.Avances;
using GrhDz.Apps.Dettes;
using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.Avances;
using GrhDz.Domains.Models.Dettes;
using GrhDz.Domains.Models.Employees;
using GrhDz.Domains.Models.Primes;
using GrhDz.Domains.Models.Remboursements;
using Implementation.Services.Avance;
using Implementation.Services.Prime;
using Implementation.Services.Remboursement;
using Microsoft.AspNetCore.Components;

namespace Gestion_personal.Components.Layout.Dettes
{
    public partial class DetteAvanceEmployeFrom
	{
	
    [Parameter] public bool IsVisibleFicheAvanceDette { get; set; }
		[Parameter] public EventCallback OnClose { get; set; }
		[Parameter] public int EmployeID { get; set; }
        [Parameter] public DateTime Date { get; set; }
		[Inject] private IAvanceService AvanceService { get; set; }
		[Inject] private IDetteService DetteService { get; set; }
		[Inject] private IRemboursementService remboursementService { get; set; }
        [Inject] private IPrimeService PrimeService { get; set; }

        private List<Employe> employes;
		private Result<List<AvanceModel>> avances;
		private Result<List<Dette>> dettes;
		private Result<List<PrimeType>> primes;
        private Result<List<RemboursementType>> remboursements;
        int selectedIndex = 0;

        protected override async Task OnParametersSetAsync()
		{
			if (EmployeID > 0)
			{
				await LoadAvancesAndDettes();
			}
		}

		private async Task LoadAvancesAndDettes()
		{
			try
			{
				avances = await AvanceService.GeAvanceByEmployeId(EmployeID, Date);
				dettes = await DetteService.GetByEmployeIdAsync(EmployeID, Date);
                remboursements = await remboursementService.GetByEmployeIdInMonthAsync(EmployeID, Date);
				primes = await PrimeService.GetByEmployeIdInMonth(EmployeID,Date);

            }
			catch (Exception ex)
			{
				Console.WriteLine($"Error loading data: {ex.Message}");
			}
		}

		private async Task Hide_Popup_FicheAvanceDette()
		{
			if (OnClose.HasDelegate)
				await OnClose.InvokeAsync();
		}

	}
}