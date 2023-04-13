using Microsoft.AspNetCore.Mvc;
using SharedModels.Response.Enums;
using SharedModels.Response;
using System.Net;
using StocksApi.Models.Request;
using SharedMethods.Logging;
using SharedModels.Response.Constants;
using StockService.Models;
using StockService;

namespace StocksApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class StocksController : ControllerBase
    {
        private readonly IStockService _stockService;
        private ILogger<StocksController> _logger { get; set; }

        public StocksController(IStockService stockService, ILogger<StocksController> logger)
        {
            _stockService = stockService ?? throw new ArgumentNullException(nameof(stockService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        

        /// <summary>
        /// Get stocks method
        /// </summary>
        /// <param name="requestStocksDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]", Name = "GetStocks")]
        [ProducesResponseType(typeof(ActionResult<List<StockDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ActionResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ActionResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<List<StockDto>>> GetStocks([FromForm] RequestStocksDTO requestStocksDTO)
        {
            try
            {

                return Ok(await _stockService.GetStocks(requestStocksDTO.NumberOfItems,
                    requestStocksDTO.Page,
                    requestStocksDTO.Filter,
                    requestStocksDTO.OrderOrientation.GetValueOrDefault(),
                    requestStocksDTO.FieldToOrderCase.GetValueOrDefault()));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.GetGenericError(nameof(GetStocks)));

                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPatch]
        [Route("[action]", Name = "UpdateStock")]
        [ProducesResponseType(typeof(ActionResult<GenericResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ActionResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ActionResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<GenericResponse>> UpdateStock([FromBody] StockDto updateStockDto)
        {
            try
            {
                var result = await _stockService.UpdateStock(updateStockDto);

                var genericReponse = new GenericResponse
                {
                    IsSuccess = result,
                    Message = result ? ResponseMessages.UpdateSuccessful : ResponseMessages.UpdateNotSuccessful,
                    ResponseStatus = result ? ResponseStatusEnum.OK : ResponseStatusEnum.ERRORUPDATING
                };

                if (result)
                {
                    return Ok(genericReponse);
                }
                else if (!result && genericReponse.ResponseStatus.Equals(ResponseStatusEnum.ERRORUPDATING))
                {
                    return BadRequest(ResponseStatusEnum.ERRORUPDATING.ToString());
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, genericReponse.ResponseStatus.ToString());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.GetGenericError(nameof(InsertStock)));

                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpDelete]
        [Route("[action]", Name = "DeleteStock")]
        [ProducesResponseType(typeof(ActionResult<GenericResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ActionResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ActionResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<GenericResponse>> DeleteStock([FromBody] RequestStockDeleteDto requestStockDeleteDto)
        {
            try
            {
                var result = await _stockService.DeleteStock(requestStockDeleteDto.ProductSku);

                var genericReponse = new GenericResponse
                {
                    IsSuccess = result,
                    Message = result ? ResponseMessages.DeleteSuccessful : ResponseMessages.DeleteNotSuccessful,
                    ResponseStatus = result ? ResponseStatusEnum.OK : ResponseStatusEnum.ERRORDELETING
                };

                if (result)
                {
                    return Ok(genericReponse);
                }
                else if (!result && genericReponse.ResponseStatus.Equals(ResponseStatusEnum.ERRORDELETING))
                {
                    return BadRequest(ResponseStatusEnum.ERRORDELETING.ToString());
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, genericReponse.ResponseStatus.ToString());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.GetGenericError(nameof(DeleteStock)));
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Method To Insert One Stock
        /// </summary>
        /// <param name="insertStocksDTO"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]", Name = "InsertStock")]
        [ProducesResponseType(typeof(ActionResult<GenericResponse>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ActionResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ActionResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<GenericResponse>> InsertStock([FromBody] StockDto insertStocksDTO)
        {
            try
            {
                var result = await _stockService.InsertStock(insertStocksDTO);

                if (result.IsSuccess)
                {
                    return CreatedAtAction(nameof(InsertStock), result);
                }
                else if (!result.IsSuccess && result.ResponseStatus.Equals(ResponseStatusEnum.INVALIDSTOCKDUPLICATE))
                {
                    return BadRequest(ResponseStatusEnum.INVALIDSTOCKDUPLICATE.ToString());
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, result.ResponseStatus.ToString());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.GetGenericError(nameof(InsertStock)));

                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
