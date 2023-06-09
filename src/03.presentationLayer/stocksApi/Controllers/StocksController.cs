﻿using Microsoft.AspNetCore.Mvc;
using SharedModels.Response.Enums;
using SharedModels.Response;
using System.Net;
using StocksApi.Models.Request;
using SharedMethods.Logging;
using SharedModels.Response.Constants;
using StockService.Models;
using StockService;
using Microsoft.AspNetCore.Authorization;
using SharedModels.User.Enum;
using StocksApi.Utils.AuthorizationRoles;

namespace StocksApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
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
    [Authorize]
    [ProducesResponseType(typeof(ActionResult<List<StockDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ActionResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ActionResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ActionResult), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ActionResult), StatusCodes.Status500InternalServerError)]
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
    [AuthorizeRoles(UserTypeEnum.Admin, UserTypeEnum.StockManager)]
    [ProducesResponseType(typeof(ActionResult<GenericResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ActionResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ActionResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ActionResult), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ActionResult), StatusCodes.Status500InternalServerError)]
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
    [AuthorizeRoles(UserTypeEnum.Admin, UserTypeEnum.StockManager)]
    [ProducesResponseType(typeof(ActionResult<GenericResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ActionResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ActionResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ActionResult), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ActionResult), StatusCodes.Status500InternalServerError)]
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
    [AuthorizeRoles(UserTypeEnum.Admin, UserTypeEnum.StockManager, UserTypeEnum.User)]
    [ProducesResponseType(typeof(ActionResult<GenericResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ActionResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ActionResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ActionResult), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ActionResult), StatusCodes.Status500InternalServerError)]
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
