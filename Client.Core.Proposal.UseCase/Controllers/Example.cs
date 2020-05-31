﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Client.Core.Proposal.UseCase.Models;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Client.Core.Proposal.UseCase.Controllers
{
    [Route("")]
    public class Example : ControllerBase
    {
        private ILogger Logger { get; }
        private IInfluxDBClient Client { get; }
        private IWriteApi WriteApi { get; }
        private IMeasurementMapper MeasurementMapper { get; }
        private InfluxSettings Settings { get; }

        public Example(
            ILogger<Example> logger,
            IInfluxDBClient client, 
            IWriteApi writeApi,
            IMeasurementMapper measurementMapper,
            InfluxSettings settings)
        {
            Logger = logger;
            Client = client;
            WriteApi = writeApi;
            MeasurementMapper = measurementMapper;
            Settings = settings;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginMeasurement measurement)
        {
            try
            {
                await WriteApi.WriteMeasurementAsync(WritePrecision.S, measurement);
                return NoContent();
            }
            catch (Exception exception)
            {
                // logging and capturing the actual return/status codes etc.
                return BadRequest();
            }
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutMeasurement measurement)
        {
            try
            {
                var writeApi = Client.GetWriteApi();
                await writeApi.WriteMeasurementAsync(WritePrecision.S, measurement);
                return NoContent();
            }
            catch (Exception exception)
            {
                // logging and capturing the actual return/status codes etc.
                return BadRequest();
            }
        }

        [HttpPost("usage")]
        public async Task<IActionResult> Usage([FromBody] UsageMeasurement measurement)
        {
            try
            {
                 var record = MeasurementMapper
                     .ToPoint(measurement, WritePrecision.Ns)
                     .ToLineProtocol();
                 Logger.LogDebug("Logging Usage: {record}", record);
                
                await WriteApi.WriteRecordAsync(WritePrecision.S, record);
                return NoContent();
            }
            catch (Exception exception)
            {
                // logging and capturing the actual return/status codes etc.
                return BadRequest();
            }
        }

        [HttpGet("temperature/{temp}")]
        public async Task<IActionResult> Temperature([FromRoute] float temp)
        {
            var data = PointData.Measurement("weather").Field("temperature", temp);

            var client = InfluxDBClientFactory.Create(builder =>
            {
                builder
                    .Url(Settings.Url)
                    .AuthenticateToken(Settings.Token)
                    .AddDefaultTag("station", "weather-station-1");
            });

            var writeApi = client.GetWriteApi();
            await writeApi.WritePointAsync("weather-bucket", Settings.Org, data);

            return NoContent();

        }

    }
}
