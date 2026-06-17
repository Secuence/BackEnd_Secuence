using Microsoft.EntityFrameworkCore.Metadata.Conventions;

using System.Runtime.InteropServices;

namespace SecuenceBack.Models
{
    public class KPISDashboard
    {
        public ApplicationsDashboardKPIS AplicationsKpis { get; set; }
        public CandidateDashboardKPIS candidateKpis {get;set;}

        public List<Funnel>? funnel {  get; set; } 
    }
    public class Funnel {

        public int ApplicationId { get; set; }
        public string position { get; set; }
        public FunnelCounters FunnelCounters { get; set; }

    }
    public class FunnelCounters
    {
        public int NuevoCandidato { get; set; }
        public int CandidatoRechazado { get; set; }
        public int CandidatoAprobadoInterno { get; set; }
        public int CandidatoAprobadoManager { get; set; }
        public int CandidatoEnviadoCliente { get; set; }
        public int CandidatoEnRevisionCliente { get; set; }
        public int CandidatoAprobadoCliente { get; set; }
        public int CandidatoContradados { get; set; }
        public int CandidatosOfertaRechazada { get; set; }
        public int CandidatosOfertaRespuesta { get; set; }
        public int TotalCandidatos { get; set; }

    }
    public class ApplicationsDashboardKPIS
    {
        public int requisicionEliminada { get; set; }
		public int requisicionCreado { get; set; }
        public int requisicionAprobado { get; set; }
        public int requisicionRechazado { get; set; }
        public int requisicionActivas { get; set; }
        public int requisicionCerrado { get; set; }
        public int Totalrequisicion { get; set; }
    }
    public class CandidateDashboardKPIS
    {
        public int NuevoCandidato { get; set; }
         public int CandidatoRechazado { get; set; }
        public int CandidatoAprobadoInterno { get; set; }
        public int CandidatoAprobadoManager { get; set; }
        public int CandidatoEnviadoCliente { get; set; }
        public int CandidatoEnRevisionCliente { get; set; }
        public int CandidatoAprobadoCliente { get; set; }
        public int CandidatoContradados { get; set; }
        public int CandidatosOfertaRechazada { get; set; }
        public int CandidatosOfertaRespuesta { get; set; }
        public int  TotalCandidatos { get; set; }
    }
    public class HistoricKPis
    { 
        public int? timepoEnLlenar { get; set; }
        public int? TotalPostulaciones { get; set; }
        public int? PromedioAprobacion { get; set; }
        public int? PromedioPublicacion { get; set; }
        public int? PromedioCierre { get; set; }
        public int? Promediorechazo { get; set; }
        public int? rechazoPorCliente { get; set; }
        public int? seleccionadoPorCliente { get; set; }
        public HistoricPosicionesVs? posicionesVs {  get; set; }
        public HistoricPromedoEtapa? promedioEtapa { get; set; }
        public List<HistoricTimeToHire?> timeToHire { get; set; }
        public HistoricMotivosCierre? motivosCierre { get; set; }
        public HistoricTasaConversion? tasaConversion { get; set; }
        public HistoricCandidatoInterno? candidatoInterno { get; set; }
        public List<HistoricRatioToHire?> ratioToHire { get; set; }
    }
    public class HistoricPosicionesVs
    { 
    public int? totalPosicionesCubiertasm {  get; set; }
        public int? totalPosicioneNocubiertas {  get; set; }
    }
    public class HistoricMotivosCierre
    { 
        public int? motivoCierreCancelada { get; set; }
        public int? motivoCierreFaltaDeSeguimiento { get; set; }
        public int? motivoCierreCubiertaPorOtroPRov {  get; set; }
        public int? motivoCierreparcialmente { get; set; }
}
    public class HistoricTasaConversion
    { 
    public int? tasaCreados { get; set; }
        public int? tasaRevisionHR { get; set; }
        public int? tasaEntrevistados {  get; set; }
        public int? tasaManager {  get; set; }
        public int? tasaEnviadosCliente { get; set; }
        public int? tasaEntrevistadosCliente { get; set; }
        public int? tasaAprobadoTrasEntrevistaConCliente { get; set; }
        public int? tasaOfertasEnviadas {  get; set; }
        public int? tasaContratados {  get; set; }
    }
    public class HistoricCandidatoInterno
    {
        public int? fuenteExterno { get; set; }
        public int? fuenteInterno { get; set; }
    }
    public class HistoricPromedoEtapa
    {
        public int? PromedioRevisionInterna { get; set; }
        public int? PromedioEntrevista { get; set; }
        public int? PromedioManager { get; set; }
        public int? PromediorEnviadoCliente { get; set; }
        public int? PromedioRespuestaCliente { get; set; }
        public int? PromedioAprobadoTrasEntrevistaConCliente { get; set; }
        public int? PromedioCartaDeOferta { get; set; }
        public int? PromedioContratacion { get; set; }
    }
    public class HistoricTimeToHire
    { 
        public string? position { get; set; }
        public int? timeToHire { get; set; }
    }
    public class HistoricRatioToHire
    {
        public string? position { get; set; }
        public decimal? ratioCandidate { get; set; }
    }
    public class Semaforo
    {
        public int id { get; set; }
        public string position { get; set; }
        public  int status { get; set; }
        public int dias { get; set; }
        public bool hold { get; set; }
        public int diasHold { get; set; }
        public string client { get; set; }
    }
    public class Vacantkpis
    { 
       public int? days { get; set; }
        public int? holdDays { get; set; }
        public int? interRevisionCandidates { get; set; }
        public int? deliveryRevisionCandidates { get; set; }
        public int? clientRevisionCandidates {  get; set; }
        
    }
    public class HistoricPosition
    { 
       public int? positionid { get; set; }
       public string? positionname { get; set; }
    }


    public class HistoricSingleMetrics
    { 
        public HistoricPromedoEtapa? singlePromedioEtapa { get; set; }
        public HistoricTasaConversion? singleTasaConversion { get; set; }
        public HistoricCandidatoInterno? singleCandidatoFuentes { get; set; }
        public HistoricRatioToHire? singleRatioToHire { get; set; }
    }
}
