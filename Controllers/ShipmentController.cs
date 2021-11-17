using Direct4me.Data;
using Direct4me.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Linq;

namespace Direct4me.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShipmentController : ControllerBase
    {
        private readonly ShipmentContext _context;
        private IDbContextTransaction Transaction { get; set; }

        public ShipmentController(ShipmentContext context)
        {
            _context = context;
        }

        /// <summary>
        /// save scanned shipment
        /// </summary>
        /// <param name="barCode">barCode of shippment</param>
        /// <response code="200">Scanned shipment successfully saved</response>
        /// <response code="400">An error occurred while saving scanned shipment</response>
        [HttpPost("saveScannedShipment")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult SaveScannedShipment(string barCode)
        {
            using (Transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var shipment = new Shipment()
                    {
                        BarCode = barCode
                    };

                    _context.Add(shipment);
                    _context.SaveChanges();
                    Transaction.Commit();

                    return Ok();
                }
                catch (Exception e)
                {
                    Transaction.Rollback();
                    return BadRequest(e.Message);
                }
            }
        }

        /// <summary>
        /// get total number of all scanned shipments
        /// </summary>
        /// <response code="200">Total number of all scanned shipments successfully obtained</response>
        /// <response code="400">An error occurred while getting total number of all scanned shipments</response>
        [HttpGet("getScannedShipmentsCount")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<int> GetScannedShipmentsCount()
        {
            try
            {
                return Ok(_context.Shipment.Count());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// get total number of all scanned shipments by delivery service
        /// </summary>
        /// <param name="deliveryService">delivery service name</param>
        /// <response code="200">Total number of all scanned shipments by delivery service successfully obtained</response>
        /// <response code="400">An error occurred while getting total number of all scanned shipments by delivery service</response>
        [HttpGet("getScannedShipmentsByDeliveryServiceCount")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<int> GetScannedShipmentsByDeliveryServiceCount(string deliveryService)
        {
            try
            {
                string deliveryServiceCode;
                switch (deliveryService.ToLower())
                {
                    case "dpd":
                        deliveryServiceCode = "1696";
                        break;
                    case "gls":
                        deliveryServiceCode = "GL";
                        break;
                    case "posta":
                        deliveryServiceCode = "PS00";
                        break;
                    case "ups":
                        deliveryServiceCode = "UP";
                        break;
                    default:
                        deliveryServiceCode = string.Empty;
                        break;
                }

                return Ok(_context.Shipment.Where(x => x.BarCode.StartsWith(deliveryServiceCode)).Count());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
