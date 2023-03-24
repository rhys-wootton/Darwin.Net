﻿using DarwinNet.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarwinTests.Requests
{
    [TestFixture]
    internal class GetArrivalBoardAsyncTest : BaseTest
    {
        [Test]
        public async Task GetArrivalBoardAsync_ValidParams_ReturnsStationBoardWithValidData()
        {
            // Arrange
            int numRows = 50;
            Station station = Station.LondonStPancrasInternational;
            TimeSpan timeWindow = TimeSpan.FromMinutes(60);
            DateTime requestTimeWindow = DateTime.UtcNow.Add(timeWindow);

            // Act
            var result = await Darwin.Requests.GetArrivalBoardAsync(numRows, station, timeWindow);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.GetType(), Is.EqualTo(typeof(StationBoard)));
                Assert.That(result.Station, Is.EqualTo(station), "Returned data does not match expected station.");
                Assert.That(result.TrainServices?.Count, Is.LessThanOrEqualTo(numRows), "Number of TrainServices? returned exceeds numRows parameter.");
                Assert.That(result.TrainServices?.All(service => service.ScheduledTimeArrival?.Time <= requestTimeWindow), Is.True, "Returned TrainServices? have services past the defined time window.");
            });
        }

        [Test]
        public void GetArrivalBoardAsync_InvalidNumRows_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            int numRows = -1;
            Station station = Station.LondonStPancrasInternational;
            TimeSpan timeWindow = TimeSpan.FromMinutes(60);

            // Act and Assert
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => Darwin.Requests.GetArrivalBoardAsync(numRows, station, timeWindow));
        }

        [Test]
        public void GetArrivalBoardAsync_InvalidTimeWindow_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            int numRows = 50;
            Station station = Station.LondonStPancrasInternational;
            TimeSpan timeWindow = TimeSpan.FromMinutes(-150);

            // Act and Assert
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => Darwin.Requests.GetArrivalBoardAsync(numRows, station, timeWindow));
        }

        [Test]
        public async Task GetArrivalBoardAsync_FilterStation_ReturnsStationBoardWithValidData()
        {
            // Arrange
            int numRows = 50;
            Station station = Station.LondonStPancrasInternational;
            TimeSpan timeWindow = TimeSpan.FromMinutes(60);
            Station filterStation = Station.LondonBridge;
            DateTime requestTimeWindow = DateTime.UtcNow.Add(timeWindow);

            // Act
            var result = await Darwin.Requests.GetArrivalBoardAsync(numRows, station, timeWindow, filterStation: filterStation);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.GetType(), Is.EqualTo(typeof(StationBoard)));
                Assert.That(result.Station, Is.EqualTo(station), "Returned data does not match expected station.");
                Assert.That(result.FilterStation, Is.EqualTo(filterStation), "Returned data does not match expected station.");
                Assert.That(result.TrainServices?.Count, Is.LessThanOrEqualTo(numRows), "Number of TrainServices? returned exceeds numRows parameter.");
                Assert.That(result.TrainServices?.All(service => service.ScheduledTimeArrival?.Time <= requestTimeWindow), Is.True, "Returned TrainServices? have services past the defined time window.");
            });
        }

        [Test]
        public async Task GetArrivalBoardAsync_FilterType_ReturnsStationBoardWithValid()
        {
            int numRows = 50;
            Station station = Station.LondonStPancrasInternational;
            TimeSpan timeWindow = TimeSpan.FromMinutes(60);
            Station filterStation = Station.LondonBridge;
            FilterType filterType = FilterType.From;
            DateTime requestTimeWindow = DateTime.UtcNow.Add(timeWindow);

            // Act
            var result = await Darwin.Requests.GetArrivalBoardAsync(numRows, station, timeWindow, filterType: filterType, filterStation: filterStation);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.GetType(), Is.EqualTo(typeof(StationBoard)));
                Assert.That(result.Station, Is.EqualTo(station), "Returned data does not match expected station.");
                Assert.That(result.FilterStation, Is.EqualTo(filterStation), "Returned data does not match expected station.");
                Assert.That(result.TrainServices?.Count, Is.LessThanOrEqualTo(numRows), "Number of TrainServices? returned exceeds numRows parameter.");
                Assert.That(result.TrainServices?.All(service => service.ScheduledTimeArrival?.Time <= requestTimeWindow), Is.True, "Returned TrainServices? have services past the defined time window.");
            });
        }

        [Test]
        public async Task GetArrivalBoardAsync_TimeOffset_ReturnsStationBoardWithValidData()
        {
            // Arrange
            int numRows = 50;
            Station station = Station.LondonStPancrasInternational;
            TimeSpan timeWindow = TimeSpan.FromMinutes(60);
            TimeSpan timeOffset = TimeSpan.FromMinutes(-30);
            DateTime requestTimeWindow = DateTime.UtcNow.Add(timeWindow).Add(timeOffset);

            // Act
            var result = await Darwin.Requests.GetArrivalBoardAsync(numRows, station, timeWindow, timeOffset: timeOffset);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.GetType(), Is.EqualTo(typeof(StationBoard)));
                Assert.That(result.Station, Is.EqualTo(station), "Returned data does not match expected station.");
                Assert.That(result.TrainServices?.Count, Is.LessThanOrEqualTo(numRows), "Number of TrainServices? returned exceeds numRows parameter.");
                Assert.That(result.TrainServices?.All(service => service.ScheduledTimeArrival?.Time <= requestTimeWindow), Is.True, "Returned TrainServices? have services past the defined time window.");
            });
        }
    }
}