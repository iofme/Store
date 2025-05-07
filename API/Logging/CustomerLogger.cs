using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Logging
{
    public class CustomerLogger : ILogger
    {
        readonly string loggerName;

        readonly CustomLoggerProviderConfig loggerConfig;

        public CustomerLogger(string name, CustomLoggerProviderConfig config)
        {
            loggerName = name;
            loggerConfig = config;
        }


        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == loggerConfig.LogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            string messagem = $"{logLevel.ToString()}: {eventId.Id} - {formatter(state, exception)}";

            EscreverTextoNoArquivo(messagem);
        }

        private void EscreverTextoNoArquivo(string messagem)
        {
            string caminhoArquivoLog = @"C:\Users\joao.burigo\Desktop\Project\store\Macoratti_Log.txt";

            using (StreamWriter streamWriter = new StreamWriter(caminhoArquivoLog, true))
            {
                try{
                    streamWriter.WriteLine(messagem);
                    streamWriter.Close();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}