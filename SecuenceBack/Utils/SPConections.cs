using SecuenceBack.Controllers;
using SecuenceBack.Models;
using SecuenceBack.Utils;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using System.Globalization;

namespace SecuenceBack.Utils
{
    public class SPConections
    {
        private readonly IConfiguration _appSettings;
        public SPConections(IConfiguration configuration)
        {
            _appSettings = configuration;
        }

        public async Task<List<TalentPollDTO>> SPGetTalentPullstring(int skip, int pagSize, string connectionString, int? status, DateTime? intialDate, DateTime? finalDate, string? position, string? department, string? client, string? recruiter, List<string>? TagsType, string? namefilter)
        {
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            using SqlConnection sql = new(connectionString);
            SqlCommand cmd;

            if (_appSettings["ConnectionStrings:isDev"] == "true")
            {
                cmd = new SqlCommand("SP_GetAllCandidatesDev", sql);
            }
            else
            {
                cmd = new SqlCommand("SP_GetAllCandidates", sql);
            }
            List<TalentPollDTO> talentPoll = new List<TalentPollDTO>();

            using (cmd)
            {
                var tagType = TagsTableType(TagsType);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@pagSize", pagSize));
                cmd.Parameters.Add(new SqlParameter("@pagNumber", skip));
                cmd.Parameters.Add(new SqlParameter("@status", status ?? (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@intialDate", intialDate ?? (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@finalDate", finalDate ?? (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@position", position ?? (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@department", department ?? (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@client", client ?? (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@recruiter", recruiter ?? (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@TagsType", tagType)).TypeName = "dbo.TagsType";
                cmd.Parameters.Add(new SqlParameter("@name", namefilter ?? (object)DBNull.Value));

                await sql.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                if (reader != null)
                {
                    while (await reader.ReadAsync())
                    {
                        var candidate = new TalentPollDTO()
                        {
                            idCandidate = (int)reader["id"],
                            firstName = textInfo.ToTitleCase(reader["firstName"].ToString().ToLower()),
                            lastName = textInfo.ToTitleCase(reader["lastName"].ToString().ToLower()),
                            created = (DateTime)reader["createdAt"],
                            status = (int)reader["status"],
                            totalRec = (int)reader["totalRec"],
                            position = reader["position"].ToString(),
                            positionId = (int)reader["positionId"],
                            client = reader["client"].ToString(),
                            applicationStatus = (int)reader["apStatus"],
                        };
                        talentPoll.Add(candidate);
                    }
                }
            }
            return talentPoll;
        }
        private DataTable TagsTableType(List<string> tags)
        {
            DataTable tagsTable = new();
            tagsTable.Columns.Add("tagsName", typeof(string)).AllowDBNull = true;

            foreach (var tag in tags)
            {
                DataRow row = tagsTable.NewRow();
                row["tagsName"] = tag;
                tagsTable.Rows.Add(row);
            }
            return tagsTable;
        }

        public async Task<KPISDashboard> SPGetDashboardKPIS(string connectionString, int? recluterFilter, string? clientFilter, bool? cerradas)
        {
            using SqlConnection sql = new(connectionString);
            KPISDashboard kPIS = new KPISDashboard();
            kPIS.AplicationsKpis = new ApplicationsDashboardKPIS();
            kPIS.candidateKpis = new CandidateDashboardKPIS();
            kPIS.funnel = new List<Funnel>();
            SqlCommand cmd;

            if (_appSettings["ConnectionStrings:isDev"] == "true")
            {
                cmd = new SqlCommand("SP_GetDashBoadKPISDev", sql);
            }
            else
            {
                cmd = new SqlCommand("SP_GetDashBoadKPIS", sql);
            }
            using (cmd)
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@RecluterFilter", recluterFilter ?? (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@ClientFilter", clientFilter ?? (object)DBNull.Value));
                await sql.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                if (reader != null)
                {
                    while (await reader.ReadAsync())
                    {

                        //Torta
                        kPIS.AplicationsKpis.requisicionEliminada = (int)reader["requisicionEliminada"];
                        kPIS.AplicationsKpis.requisicionCreado = (int)reader["requisicionCreado"];
                        kPIS.AplicationsKpis.requisicionAprobado = (int)reader["requisicionAprobado"];
                        kPIS.AplicationsKpis.requisicionRechazado = (int)reader["requisicionRechazado"];
                        kPIS.AplicationsKpis.requisicionActivas = (int)reader["requisicionActivas"];
                        if (cerradas == true)
                        {
                            kPIS.AplicationsKpis.requisicionCerrado = (int)reader["requisicionCerrado"];
                        }
                        else
                        {
                            kPIS.AplicationsKpis.requisicionCerrado = 0;
                        }
                        kPIS.AplicationsKpis.Totalrequisicion = (int)reader["Totalrequisicion"];

                        //Contadores

                        kPIS.candidateKpis.NuevoCandidato = (int)reader["NuevoCandidato"];    //
                        kPIS.candidateKpis.CandidatoAprobadoInterno = (int)reader["CandidatoAprobadoInterno"]; ///
                        kPIS.candidateKpis.CandidatoEnviadoCliente = (int)reader["CandidatoEnviadoCliente"];   ///
                        kPIS.candidateKpis.CandidatosOfertaRespuesta = (int)reader["CandidatosOfertaRespuesta"];  ////

                        // Convertir el JSON string en un objeto (suponiendo que el JSON tiene un formato adecuado)
                        string funnelJson = reader["funnel"] as string;
                        if (!string.IsNullOrEmpty(funnelJson))
                        {
                            kPIS.funnel = JsonConvert.DeserializeObject<List<Funnel>>(funnelJson);


                        }
                    }
                }
            }
            return kPIS;
        }

        public async Task<List<LogsDTo>> SPGetDashboardLogs(string connectionString, int? pageNumber, int? pageSize, DateTime? intialDate, DateTime? finalDate, string? type = null)
        {
            using SqlConnection sql = new(connectionString);
            List<LogsDTo> logs = new List<LogsDTo>();
            SqlCommand cmd;

            if (_appSettings["ConnectionStrings:isDev"] == "true")
            {
                cmd = new SqlCommand("SP_GetAllLogsDev", sql);
            }
            else
            {
                cmd = new SqlCommand("SP_GetAllLogs", sql);
            }
            using (cmd)
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@pageNumber", pageNumber ?? (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@pageSize", pageSize ?? (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@Type", type ?? (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@intialDate", intialDate ?? (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@finalDate", finalDate ?? (object)DBNull.Value));
                await sql.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                if (reader != null)
                {
                    while (await reader.ReadAsync())
                    {
                        LogsDTo logsdb = new LogsDTo();
                        logsdb.userName = reader["userName"].ToString();
                        logsdb.moduleName = reader["moduleName"].ToString();
                        logsdb.action = reader["action"].ToString();
                        logsdb.message = reader["message"].ToString();
                        logsdb.actionDate = (DateTime)reader["actionDate"];
                        logs.Add(logsdb);
                    }
                }
            }
            return logs;
        }

        public async Task<List<Semaforo>> SPGetSemaforo(string connectionString, string status, bool? hold, string client)
        {
            using SqlConnection sql = new(connectionString);
            int? dbstatus = -1;
            if (status != null)
                dbstatus = Enums.StatusApplication.GetValueOrDefault(status, -1);
            List<Semaforo> semaforos = new List<Semaforo>();


            SqlCommand cmd;

            if (_appSettings["ConnectionStrings:isDev"] == "true")
            {
                cmd = new SqlCommand("SP_GetSemaforo_2Dev", sql);
            }
            else
            {
                cmd = new SqlCommand("SP_GetSemaforo_2", sql);
            }
            using (cmd)
            {

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@Status", dbstatus == -1 ? ((object)DBNull.Value) : dbstatus));
                cmd.Parameters.Add(new SqlParameter("@Hold", hold));
                cmd.Parameters.Add(new SqlParameter("@Client", client));
                await sql.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                if (reader != null)
                {
                    while (await reader.ReadAsync())
                    {
                        Semaforo semaforo = new Semaforo();
                        semaforo.id = (int)reader["id"];
                        semaforo.position = reader["position"].ToString();
                        semaforo.client = reader["client"].ToString();
                        semaforo.status = (int)reader["status"];
                        semaforo.dias = !reader.IsDBNull(reader.GetOrdinal("days")) ? (int)reader["days"] : 0;
                        semaforo.diasHold = !reader.IsDBNull(reader.GetOrdinal("daysHold")) ? (int)reader["daysHold"] : 0;
                        semaforo.hold = (bool)reader["hold"];
                        semaforos.Add(semaforo);
                    }
                    //semaforos.Add(semaforo); // borrar luego de que eder implemnte el codigo
                }
            }
            return semaforos;
        }

        public async Task<HistoricKPis> SPGethistoric(string connectionString, string? client, string? department, string? position, DateTime? stratTime, DateTime? endTime)
        {
            using SqlConnection sql = new(connectionString);

            HistoricKPis historico = new HistoricKPis();
            historico.posicionesVs = new HistoricPosicionesVs();
            historico.motivosCierre = new HistoricMotivosCierre();
            historico.tasaConversion = new HistoricTasaConversion();
            historico.candidatoInterno = new HistoricCandidatoInterno();
            historico.promedioEtapa = new HistoricPromedoEtapa();
            historico.timeToHire = new List<HistoricTimeToHire?>();
            historico.ratioToHire = new List<HistoricRatioToHire?>();

            SqlCommand cmd;
            SqlCommand cmd2;
            SqlCommand cmd3;
            if (_appSettings["ConnectionStrings:isDev"] == "true")
            {
                cmd = new SqlCommand("SP_GetHistoricDev", sql);
                cmd2 = new SqlCommand("SP_GetTimeToHireDev", sql);
                cmd3 = new SqlCommand("SP_GetRatioToHireDev", sql);
            }
            else
            {
                cmd = new SqlCommand("SP_GetHistoric", sql);
                cmd2 = new SqlCommand("SP_GetTimeToHire", sql);
                cmd3 = new SqlCommand("SP_GetRatioToHire", sql);
            }
            using (cmd)
            {

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Client", client ?? (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@Department", department ?? (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@Position", position ?? (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@StartTime", stratTime ?? (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@EndTime", endTime ?? (object)DBNull.Value));
                await sql.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();
                if (reader != null)
                {
                    while (await reader.ReadAsync())
                    {
                        // Metrias Base
                        historico.timepoEnLlenar = !reader.IsDBNull(reader.GetOrdinal("timepoEnLlenar")) ? (int)reader["timepoEnLlenar"] : 0;
                        historico.TotalPostulaciones = !reader.IsDBNull(reader.GetOrdinal("TotalPostulaciones")) ? (int)reader["TotalPostulaciones"] : 0;
                        historico.PromedioAprobacion = !reader.IsDBNull(reader.GetOrdinal("PromedioAprobacion")) ? (int)reader["PromedioAprobacion"] : 0;
                        historico.PromedioPublicacion = !reader.IsDBNull(reader.GetOrdinal("PromedioPublicacion")) ? (int)reader["PromedioPublicacion"] : 0;
                        historico.PromedioCierre = !reader.IsDBNull(reader.GetOrdinal("PromedioCierre")) ? (int)reader["PromedioCierre"] : 0;
                        historico.Promediorechazo = !reader.IsDBNull(reader.GetOrdinal("Promediorechazo")) ? (int)reader["Promediorechazo"] : 0;
                        historico.rechazoPorCliente = !reader.IsDBNull(reader.GetOrdinal("rechazoPorCliente")) ? (int)reader["rechazoPorCliente"] : 0;
                        historico.seleccionadoPorCliente = !reader.IsDBNull(reader.GetOrdinal("seleccionadoPorCliente")) ? (int)reader["seleccionadoPorCliente"] : 0;

                        // Posiciones Cerradas vs las No Cerradas
                        historico.posicionesVs.totalPosicionesCubiertasm = !reader.IsDBNull(reader.GetOrdinal("totalPosicionesCubiertas")) ? (int)reader["totalPosicionesCubiertas"] : 0;
                        historico.posicionesVs.totalPosicioneNocubiertas = !reader.IsDBNull(reader.GetOrdinal("totalPosicioneNocubiertas")) ? (int)reader["totalPosicioneNocubiertas"] : 0;

                        // Time to Hire
                        historico.promedioEtapa.PromedioRevisionInterna = !reader.IsDBNull(reader.GetOrdinal("PromedioRevisionInterna")) ? (int)reader["PromedioRevisionInterna"] : 0;
                        historico.promedioEtapa.PromedioEntrevista = !reader.IsDBNull(reader.GetOrdinal("PromedioEntrevista")) ? (int)reader["PromedioEntrevista"] : 0;
                        historico.promedioEtapa.PromedioManager = !reader.IsDBNull(reader.GetOrdinal("PromedioManager")) ? (int)reader["PromedioManager"] : 0;
                        historico.promedioEtapa.PromediorEnviadoCliente = !reader.IsDBNull(reader.GetOrdinal("PromediorEnviadoCliente")) ? (int)reader["PromediorEnviadoCliente"] : 0;
                        historico.promedioEtapa.PromedioRespuestaCliente = !reader.IsDBNull(reader.GetOrdinal("PromedioRespuestaCliente")) ? (int)reader["PromedioRespuestaCliente"] : 0;
                        historico.promedioEtapa.PromedioAprobadoTrasEntrevistaConCliente = !reader.IsDBNull(reader.GetOrdinal("PromedioSeleccionCliente")) ? (int)reader["PromedioSeleccionCliente"] : 0;
                        historico.promedioEtapa.PromedioCartaDeOferta = !reader.IsDBNull(reader.GetOrdinal("PromedioCartaDeOferta")) ? (int)reader["PromedioCartaDeOferta"] : 0;
                        historico.promedioEtapa.PromedioContratacion = !reader.IsDBNull(reader.GetOrdinal("PromedioContratacion")) ? (int)reader["PromedioContratacion"] : 0;

                        // Motivos de Cierre
                        historico.motivosCierre.motivoCierreCancelada = !reader.IsDBNull(reader.GetOrdinal("motivoCierreCancelada")) ? (int)reader["motivoCierreCancelada"] : 0;
                        historico.motivosCierre.motivoCierreFaltaDeSeguimiento = !reader.IsDBNull(reader.GetOrdinal("motivoCierreFaltaDeSeguimiento")) ? (int)reader["motivoCierreFaltaDeSeguimiento"] : 0;
                        historico.motivosCierre.motivoCierreCubiertaPorOtroPRov = !reader.IsDBNull(reader.GetOrdinal("motivoCierreCubiertaPorOtroPRov")) ? (int)reader["motivoCierreCubiertaPorOtroPRov"] : 0;
                        historico.motivosCierre.motivoCierreparcialmente = !reader.IsDBNull(reader.GetOrdinal("motivoCierreparcialmente")) ? (int)reader["motivoCierreparcialmente"] : 0;

                        // Tasa de conversion
                        historico.tasaConversion.tasaCreados = !reader.IsDBNull(reader.GetOrdinal("tasaCreados")) ? (int)reader["tasaCreados"] : 0;
                        historico.tasaConversion.tasaRevisionHR = !reader.IsDBNull(reader.GetOrdinal("tasaRevisionHR")) ? (int)reader["tasaRevisionHR"] : 0;
                        historico.tasaConversion.tasaEntrevistados = !reader.IsDBNull(reader.GetOrdinal("tasaEntrevistados")) ? (int)reader["tasaEntrevistados"] : 0;
                        historico.tasaConversion.tasaRevisionHR = !reader.IsDBNull(reader.GetOrdinal("tasaRevisionHR")) ? (int)reader["tasaRevisionHR"] : 0;
                        historico.tasaConversion.tasaEnviadosCliente = !reader.IsDBNull(reader.GetOrdinal("tasaEnviasdosCliente")) ? (int)reader["tasaEnviasdosCliente"] : 0;
                        historico.tasaConversion.tasaEntrevistadosCliente = !reader.IsDBNull(reader.GetOrdinal("tasaEntrvistadosCliente")) ? (int)reader["tasaEntrvistadosCliente"] : 0;
                        historico.tasaConversion.tasaAprobadoTrasEntrevistaConCliente = !reader.IsDBNull(reader.GetOrdinal("tasaSeleccionadosCliente")) ? (int)reader["tasaSeleccionadosCliente"] : 0;
                        historico.tasaConversion.tasaOfertasEnviadas = !reader.IsDBNull(reader.GetOrdinal("tasaOfertasEnviadas")) ? (int)reader["tasaOfertasEnviadas"] : 0;
                        historico.tasaConversion.tasaContratados = !reader.IsDBNull(reader.GetOrdinal("tasaContratados")) ? (int)reader["tasaContratados"] : 0;

                        // Fuentes de candidatos
                        historico.candidatoInterno.fuenteExterno = !reader.IsDBNull(reader.GetOrdinal("fuenteExterno")) ? (int)reader["fuenteExterno"] : 0;
                        historico.candidatoInterno.fuenteInterno = !reader.IsDBNull(reader.GetOrdinal("fuenteInterno")) ? (int)reader["fuenteInterno"] : 0;
                    }
                }
            }
            await sql.CloseAsync();
            // Time to Hire
            using (cmd2)
            {
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.Add(new SqlParameter("@Client", client ?? (object)DBNull.Value));
                cmd2.Parameters.Add(new SqlParameter("@Department", department ?? (object)DBNull.Value));
                cmd2.Parameters.Add(new SqlParameter("@Position", position ?? (object)DBNull.Value));
                cmd2.Parameters.Add(new SqlParameter("@StartTime", stratTime ?? (object)DBNull.Value));
                cmd2.Parameters.Add(new SqlParameter("@EndTime", endTime ?? (object)DBNull.Value));
                await sql.OpenAsync();
                using var reader2 = await cmd2.ExecuteReaderAsync();
                if (reader2 != null)
                {
                    while (await reader2.ReadAsync())
                    {
                        HistoricTimeToHire timeToHire = new HistoricTimeToHire();
                        // Metrias TimeToHire

                        timeToHire.position = reader2["nameHire"].ToString();
                        timeToHire.timeToHire = !reader2.IsDBNull(reader2.GetOrdinal("promedioDiasHire")) ? (int)reader2["promedioDiasHire"] : 0;
                        historico.timeToHire.Add(timeToHire);
                    }
                }
            }
            await sql.CloseAsync();
            using (cmd3)
            {
                cmd3.CommandType = CommandType.StoredProcedure;
                cmd3.Parameters.Add(new SqlParameter("@Client", client ?? (object)DBNull.Value));
                cmd3.Parameters.Add(new SqlParameter("@Department", department ?? (object)DBNull.Value));
                await sql.OpenAsync();
                using var reader3 = await cmd3.ExecuteReaderAsync();
                if (reader3 != null)
                {
                    while (await reader3.ReadAsync())
                    {
                        HistoricRatioToHire ratioToHire = new HistoricRatioToHire();
                        // Metrias Ratio de candidato

                        ratioToHire.position = reader3["HistoricosClave"].ToString();
                        ratioToHire.ratioCandidate = !reader3.IsDBNull(reader3.GetOrdinal("ratioCandidate")) ? (decimal)reader3["ratioCandidate"] : 0;
                        historico.ratioToHire.Add(ratioToHire);
                    }
                }
            }


            return historico;
        }
        public async Task<CandidateByIdDTO> SPGetCandidateById(int id, string connectionString)
        {
            using SqlConnection sql = new(connectionString);
            CandidateByIdDTO candidate = null;


            SqlCommand cmd;
            if (_appSettings["ConnectionStrings:isDev"] == "true")
            {
                cmd = new SqlCommand("SP_GetCandidateByIdDev", sql);
            }
            else
            {
                cmd = new SqlCommand("SP_GetCandidateById", sql);
            }
            using (cmd)
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@idCandidate", id));
                await sql.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();


                if (reader != null && await reader.ReadAsync())
                {
                    candidate = new CandidateByIdDTO()
                    {
                        Id = (int)reader["id"],
                        FirstName = reader["firstName"].ToString(),
                        LastName = reader["lastName"].ToString(),
                        Email = reader["email"].ToString(),
                        CV = reader["cv"].ToString(),
                        Status = (int)reader["status"],
                        QuestionsAndAnswers = JsonConvert.DeserializeObject<List<QuestionAnswerDTO>>(reader["QuestionsAndAnswers"].ToString())
                    };
                }
                return candidate;
            }
        }

        public async Task<Vacantkpis> SPGetVacatkpis(int id, string connectionString)
        {
            using SqlConnection sql = new(connectionString);

            Vacantkpis kpi = new Vacantkpis();
            SqlCommand cmd;
            if (_appSettings["ConnectionStrings:isDev"] == "true")
            {
                cmd = new SqlCommand("SP_GetVacantsKpisDev", sql);
            }
            else
            {
                cmd = new SqlCommand("SP_GetVacantsKpis", sql);
            }
            using (cmd)
            {

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@id", id));
                await sql.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();



                if (reader != null && await reader.ReadAsync())
                {
                    kpi = new Vacantkpis()
                    {
                        days = !reader.IsDBNull(reader.GetOrdinal("days")) ? (int)reader["days"] : 0,
                        holdDays = !reader.IsDBNull(reader.GetOrdinal("holdDays")) ? (int)reader["holdDays"] : 0,
                        interRevisionCandidates = !reader.IsDBNull(reader.GetOrdinal("interRevisionCandidates")) ? (int)reader["interRevisionCandidates"] : 0,
                        deliveryRevisionCandidates = !reader.IsDBNull(reader.GetOrdinal("deliveryRevisionCandidates")) ? (int)reader["deliveryRevisionCandidates"] : 0,
                        clientRevisionCandidates = !reader.IsDBNull(reader.GetOrdinal("clientRevisionCandidates")) ? (int)reader["clientRevisionCandidates"] : 0
                    };
                }
            }
            return kpi;
        }


        public async Task<List<HistoricPosition>> SPGetHistoricPosition(string connectionString, string? client, string? department, string? position, DateTime? stratTime, DateTime? endTime)
        {
            using SqlConnection sql = new(connectionString);
            List<HistoricPosition> positionList = new List<HistoricPosition>();
            SqlCommand cmd;
            if (_appSettings["ConnectionStrings:isDev"] == "true")
            {
                cmd = new SqlCommand("SP_GetHistoricPositionsDev", sql);
            }
            else
            {
                cmd = new SqlCommand("SP_GetHistoricPositions", sql);
            }

            using (cmd)
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Client", client ?? (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@Department", department ?? (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@StartTime", stratTime ?? (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@EndTime", endTime ?? (object)DBNull.Value));
                await sql.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();
                if (reader != null)
                {
                    while (await reader.ReadAsync())
                    {
                        HistoricPosition historicPosition = new HistoricPosition();
                        // Metrias Ratio de candidato

                        historicPosition.positionname = reader["titleVacant"].ToString();
                        historicPosition.positionid= !reader.IsDBNull(reader.GetOrdinal("id")) ? (int)reader["id"] : 0;
                        positionList.Add(historicPosition);
                    }
                }
            }
            return positionList;
        }
        public async Task<List<HistoricSingleMetrics>> SPGetHistoricSingles(string connectionString, List<HistoricPosition>? positions)
        {
            using SqlConnection sql = new(connectionString);
            List<HistoricSingleMetrics> singlesList = new List<HistoricSingleMetrics>();

            SqlCommand cmd;
            if (_appSettings["ConnectionStrings:isDev"] == "true")
            {
                cmd = new SqlCommand("SP_GetSinglesHistoricsDev", sql);
            }
            else
            {
                cmd = new SqlCommand("SP_GetSinglesHistorics", sql);
            }

            using (cmd)
            {
                cmd.CommandType = CommandType.StoredProcedure;
                var positionsTable = positionTable(positions);
                cmd.Parameters.Add(new SqlParameter("@Positions", positionsTable));
                await sql.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();
                if (reader != null)
                {
                    while (await reader.ReadAsync())
                    {
                        HistoricSingleMetrics historicSingleMetrics = new HistoricSingleMetrics();
                        historicSingleMetrics.singlePromedioEtapa = new HistoricPromedoEtapa();
                        historicSingleMetrics.singleTasaConversion = new HistoricTasaConversion();
                        historicSingleMetrics.singleCandidatoFuentes = new HistoricCandidatoInterno();
                        historicSingleMetrics.singleRatioToHire = new HistoricRatioToHire();

                        // Time to Hire
                        historicSingleMetrics.singlePromedioEtapa.PromedioRevisionInterna = !reader.IsDBNull(reader.GetOrdinal("PromedioRevisionInterna")) ? (int)reader["PromedioRevisionInterna"] : 0;
                        historicSingleMetrics.singlePromedioEtapa.PromedioEntrevista = !reader.IsDBNull(reader.GetOrdinal("PromedioEntrevista")) ? (int)reader["PromedioEntrevista"] : 0;
                        historicSingleMetrics.singlePromedioEtapa.PromedioManager = !reader.IsDBNull(reader.GetOrdinal("PromedioManager")) ? (int)reader["PromedioManager"] : 0;
                        historicSingleMetrics.singlePromedioEtapa.PromediorEnviadoCliente = !reader.IsDBNull(reader.GetOrdinal("PromediorEnviadoCliente")) ? (int)reader["PromediorEnviadoCliente"] : 0;
                        historicSingleMetrics.singlePromedioEtapa.PromedioRespuestaCliente = !reader.IsDBNull(reader.GetOrdinal("PromedioRespuestaCliente")) ? (int)reader["PromedioRespuestaCliente"] : 0;
                        historicSingleMetrics.singlePromedioEtapa.PromedioAprobadoTrasEntrevistaConCliente = !reader.IsDBNull(reader.GetOrdinal("PromedioSeleccionCliente")) ? (int)reader["PromedioSeleccionCliente"] : 0;
                        historicSingleMetrics.singlePromedioEtapa.PromedioCartaDeOferta = !reader.IsDBNull(reader.GetOrdinal("PromedioCartaDeOferta")) ? (int)reader["PromedioCartaDeOferta"] : 0;
                        historicSingleMetrics.singlePromedioEtapa.PromedioContratacion = !reader.IsDBNull(reader.GetOrdinal("PromedioContratacion")) ? (int)reader["PromedioContratacion"] : 0;

                        // Tasa de conversion
                        historicSingleMetrics.singleTasaConversion.tasaCreados = !reader.IsDBNull(reader.GetOrdinal("tasaCreados")) ? (int)reader["tasaCreados"] : 0;
                        historicSingleMetrics.singleTasaConversion.tasaRevisionHR = !reader.IsDBNull(reader.GetOrdinal("tasaRevisionHR")) ? (int)reader["tasaRevisionHR"] : 0;
                        historicSingleMetrics.singleTasaConversion.tasaEntrevistados = !reader.IsDBNull(reader.GetOrdinal("tasaEntrevistados")) ? (int)reader["tasaEntrevistados"] : 0;
                        historicSingleMetrics.singleTasaConversion.tasaRevisionHR = !reader.IsDBNull(reader.GetOrdinal("tasaRevisionHR")) ? (int)reader["tasaRevisionHR"] : 0;
                        historicSingleMetrics.singleTasaConversion.tasaEnviadosCliente = !reader.IsDBNull(reader.GetOrdinal("tasaEnviasdosCliente")) ? (int)reader["tasaEnviasdosCliente"] : 0;
                        historicSingleMetrics.singleTasaConversion.tasaEntrevistadosCliente = !reader.IsDBNull(reader.GetOrdinal("tasaEntrvistadosCliente")) ? (int)reader["tasaEntrvistadosCliente"] : 0;
                        historicSingleMetrics.singleTasaConversion.tasaAprobadoTrasEntrevistaConCliente = !reader.IsDBNull(reader.GetOrdinal("tasaSeleccionadosCliente")) ? (int)reader["tasaSeleccionadosCliente"] : 0;
                        historicSingleMetrics.singleTasaConversion.tasaOfertasEnviadas = !reader.IsDBNull(reader.GetOrdinal("tasaOfertasEnviadas")) ? (int)reader["tasaOfertasEnviadas"] : 0;
                        historicSingleMetrics.singleTasaConversion.tasaContratados = !reader.IsDBNull(reader.GetOrdinal("tasaContratados")) ? (int)reader["tasaContratados"] : 0;


                        // Fuentes de candidatos
                        historicSingleMetrics.singleCandidatoFuentes.fuenteExterno = !reader.IsDBNull(reader.GetOrdinal("fuenteExterno")) ? (int)reader["fuenteExterno"] : 0;
                        historicSingleMetrics.singleCandidatoFuentes.fuenteInterno = !reader.IsDBNull(reader.GetOrdinal("fuenteInterno")) ? (int)reader["fuenteInterno"] : 0;


                        historicSingleMetrics.singleRatioToHire.position = reader["PositionName"].ToString();
                        historicSingleMetrics.singleRatioToHire.ratioCandidate = !reader.IsDBNull(reader.GetOrdinal("ratioCandidate")) ? (decimal)reader["ratioCandidate"] : 0;

                        singlesList.Add(historicSingleMetrics);
                    }
                }
            }
            return singlesList;
        }


        public static DataTable positionTable(List<HistoricPosition>? historicList)
        {
            DataTable dataTable = new();
            dataTable.Columns.Add("positionId", typeof(int));
            dataTable.Columns.Add("positionName", typeof(string));
            foreach (var position in historicList)
            {
                DataRow row = dataTable.NewRow();
                row["positionId"] = (int)position.positionid;
                row["positionName"] = !string.IsNullOrEmpty(position.positionname) ? position.positionname : DBNull.Value;
                dataTable.Rows.Add(row);
            }
            return dataTable;
        }
    }
}