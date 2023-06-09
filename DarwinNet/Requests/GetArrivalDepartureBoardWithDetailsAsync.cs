﻿using DarwinNet.Exceptions;
using DarwinNet.Factories;
using DarwinNet.Helpers;
using DarwinNet.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarwinNet.Requests
{
    public partial class Requests : IRequests
    {
        /// <summary>
        /// Returns all public arrivals and departures for the supplied station within a defined time window, including service details.
        /// </summary>
        /// <param name="numRows">The number of services to return in the resulting station board. Has to be between 0 and 10 exclusive.</param>
        /// <param name="station">The <see cref="Station"/> for which the request is being made.</param>
        /// <param name="timeWindow">How far into the past or future, in minutes (relative to <paramref name="timeOffset"/>), to return services for. Has to be between -120 and 120 minutes exclusive.</param>
        /// <param name="filterStation">The <see cref="Station"/> of either an origin or destination location to filter in.</param>
        /// <param name="filterType">The <see cref="FilterType"/> to apply. Filters services to include only those originating or terminating at the <paramref name="filterStation"/>. Defaults to "<see cref="FilterType.To"/>".</param>
        /// <param name="timeOffset">An offset in minutes against the current time to provide the station board for. Has to be between -120 and 120 minutes exclusive. </param>
        /// <returns>A <see cref="StationBoardWithDetails"/> object containing the requested details.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When numRows, timeWindow or timeOffset are out of range.</exception>
        /// <exception cref="StationCrsNullException">When station or filterStation do not have a CRS code associated with them.</exception>
        /// <exception cref="InvalidEnumArgumentException">When filterType doesn't have a valid string value associated with it.</exception>
        public Task<StationBoardWithDetails> GetArrivalDepartureBoardWithDetailsAsync(int numRows, Station station, TimeSpan timeWindow, Station? filterStation = null, FilterType filterType = FilterType.To, TimeSpan? timeOffset = null)
        {
            // Check that params are valid, if not throw exceptions or handle them elsewhere
            if (numRows <= 0 || numRows >= 10) throw new ArgumentOutOfRangeException(nameof(numRows), "numRows has to be between 0 and 10 exclusive.");
            if (timeWindow.TotalMinutes <= -120 || timeWindow.TotalMinutes >= 120) throw new ArgumentOutOfRangeException(nameof(timeWindow), "timeWindow has to be between -120 and 120 exclusive.");
            if (timeOffset != null && (timeOffset?.TotalMinutes <= -120 || timeOffset?.TotalMinutes >= 120)) throw new ArgumentOutOfRangeException(nameof(timeOffset), "timeOffset has to be between -120 and 120 exclusive.");

            var requestParams = new Dictionary<string, object?>()
            {
                { "numRows", numRows },
                { "crs", station.GetStringValue() ?? throw new StationCrsNullException(station) },
                { "filterCrs", filterStation != null ? filterStation?.GetStringValue() ?? throw new StationCrsNullException(filterStation) : null },
                { "filterType", filterType.GetStringValue() ?? throw new InvalidEnumArgumentException("filterType")},
                { "timeOffset", timeOffset?.TotalMinutes ?? 0},
                { "timeWindow", timeWindow.TotalMinutes }
            };

            return GetArrivalDepartureBoardWithDetailsInternal(requestParams);
        }

        private async Task<StationBoardWithDetails> GetArrivalDepartureBoardWithDetailsInternal(Dictionary<string, object?> requestParams)
        {
            var soapEnvelope = BuildDarwinSoapEnvelope("GetArrDepBoardWithDetails", requestParams);
            var response = await SendDarwinSoapRequestAsync(soapEnvelope, StationBoardWithDetailsFactory.Instance);
            return response;
        }
    }
}
