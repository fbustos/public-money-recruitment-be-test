using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using VacationRental.Api.Models.Requests;
using VacationRental.Api.Models.Responses;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class PostRentalTests
    {
        private readonly HttpClient _client;

        public PostRentalTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostRental_ThenAGetReturnsTheCreatedRental()
        {
            var request = new RentalBindingModel
            {
                Units = 25,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel postResult;
            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", request))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var getResponse = await _client.GetAsync($"/api/v1/rentals/{postResult.Id}"))
            {
                Assert.True(getResponse.IsSuccessStatusCode);

                var getResult = await getResponse.Content.ReadAsAsync<RentalViewModel>();
                Assert.Equal(request.Units, getResult.Units);
            }
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPutRental_ThenAPutReturnsErrorWhenThereIsNotEnoughUnits()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 2,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking1Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 2,
                Start = new DateTime(2002, 01, 01)
            };

            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
            }

            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 2,
                Start = new DateTime(2002, 01, 02)
            };

            using (var postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
            {
                Assert.True(postBooking2Response.IsSuccessStatusCode);
            }

            var putRentalRequest = new RentalBindingModel
            {
                Units = 1,
                PreparationTimeInDays = 1
            };

            var e = await Assert.ThrowsAsync<ApplicationException>(async () =>
            {
                using (await _client.PutAsJsonAsync($"/api/v1/rentals/{postRentalResult.Id}", putRentalRequest))
                {
                }
            });

            Assert.Equal("There is not enough units for existing bookings", e.Message);
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPutRental_ThenAPutReturnsErrorWhenCannotModifyPreparationTime()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 2,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking1Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 2,
                Start = new DateTime(2002, 01, 01)
            };

            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
            }

            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 2,
                Start = new DateTime(2002, 01, 04)
            };

            using (var postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
            {
                Assert.True(postBooking2Response.IsSuccessStatusCode);
            }

            var putRentalRequest = new RentalBindingModel
            {
                Units = 2,
                PreparationTimeInDays = 2
            };

            var e = await Assert.ThrowsAsync<ApplicationException>(async () =>
            {
                using (await _client.PutAsJsonAsync($"/api/v1/rentals/{postRentalResult.Id}", putRentalRequest))
                {
                }
            });

            Assert.Equal("Cannot increase preparation time because overlaps with existing bookings", e.Message);
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPutRental_ThenAPutReturnsTheUpdatedRental()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 2,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking1Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 2,
                Start = new DateTime(2002, 01, 01)
            };

            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
            }

            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 2,
                Start = new DateTime(2002, 01, 05)
            };

            using (var postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
            {
                Assert.True(postBooking2Response.IsSuccessStatusCode);
            }

            var putRentalRequest = new RentalBindingModel
            {
                Units = 1,
                PreparationTimeInDays = 2
            };

            using (var putRentalResponse = await _client.PutAsJsonAsync($"/api/v1/rentals/{postRentalResult.Id}", putRentalRequest))
            {
                Assert.True(putRentalResponse.IsSuccessStatusCode);
            }
            
            using (var getResponse = await _client.GetAsync($"/api/v1/rentals/{postRentalResult.Id}"))
            {
                Assert.True(getResponse.IsSuccessStatusCode);

                var getResult = await getResponse.Content.ReadAsAsync<RentalViewModel>();
                Assert.Equal(putRentalRequest.Units, getResult.Units);
                Assert.Equal(putRentalRequest.PreparationTimeInDays, getResult.PreparationTimeInDays);
            }
        }
    }
}