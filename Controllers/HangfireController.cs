using Hangfire;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EstudosHangFire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangfireController : ControllerBase
    {

        [HttpPost("run")]
        public IActionResult RunAutomatically()
        {
            // Executa a tarefa automaticamente
            BackgroundJob.Enqueue(() => Console.WriteLine("Atividade executada!!!"));
            return Ok("Verifique seu console, job concluido");
        }

        [HttpPost("scheduleRecurring")]
        public IActionResult ScheduleRecurringJob(int minutes)
        {
            if (minutes <= 0) return BadRequest("Informe um intervalo de tempo maior que zero!");
            // Tarefa executando recorrentemente após o tempo especificado (minutos, horas, dias, ...)
            RecurringJob.AddOrUpdate(() => Console.WriteLine($"Sua atividade com delay foi executada após {minutes} minutos"), Cron.MinuteInterval(minutes));
            return Ok($"Job será executado após {minutes} minutos recorrentemente!");
        }

        [HttpPost("scheduleJob")]
        public IActionResult ScheduleJob(int minutes)
        {
            if (minutes <= 0) return BadRequest("Informe um intervalo de tempo maior que zero!");
            // Agendamento da tarefa
            BackgroundJob.Schedule(() => Console.WriteLine($"Sua atividade com delay foi executada após {minutes} minutos"), TimeSpan.FromMinutes(minutes));
            return Ok($"Job será executado após {minutes} minutos!");
        }

        [HttpPost("chainedActivity")]
        public IActionResult RunAfterOtherJob()
        {
            // Execução sequencial de jobs, um dependente de um outro
            var parentJobId = BackgroundJob.Enqueue(() => Console.WriteLine("Atividade 'PAI' executada"));
            BackgroundJob.ContinueJobWith(parentJobId, () => Console.WriteLine("Atividade 'FILHA' executada"));
            return Ok("Job executado após a execução da dependencia. Verifique console!");
        }

    }
}
