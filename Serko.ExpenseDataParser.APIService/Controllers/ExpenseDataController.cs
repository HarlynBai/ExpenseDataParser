using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Serko.ExpenseDataParser.Abstractions;
using Microsoft.Extensions.Logging;

namespace Serko.ExpenseDataParser.APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseDataController : ControllerBase
    {
        private readonly IExpenseDataParser _expenseDataParser;
        private readonly ILogger<ExpenseDataController> _logger;

        public ExpenseDataController(ILogger<ExpenseDataController> logger, IExpenseDataParser expenseDataParser)
        {
            _logger = logger;
            _expenseDataParser = expenseDataParser;
        }
        // GET api/ExpenseData
        [HttpGet]
        public ActionResult<string> Get([FromBody]string text)
        {
            Result result = _expenseDataParser.Parse(text);
            if (!result.Error)
            {
                return Ok(result);
            }
            else
            {
                _logger.LogWarning(result.ErrorDetials);
                return BadRequest(result);
            }
        }
    }
}
