using ClickerBackend.Dtos;
using ClickerBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ClickerBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GameController : ControllerBase
    {
        IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        /// <summary>
        /// 진행 중인 게임 데이터 조회
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string userId)
        {
            GameDataDTO result = null;

            try
            {
                result = await _gameService.GetGameData(userId);
            }
            catch
            {
                return BadRequest();
            }

            return Ok(result);
        }

        /// <summary>
        /// 게임 데이터 초기화
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("init")]
        public async Task<IActionResult> Post([FromForm] string userId)
        {
            Log.Information("Initialize Game : "+userId);

            if(userId == null)
            {
                return BadRequest();
            }

            string gameId = null;

            try
            {
                await _gameService.InitializeGameData(userId, out gameId);
            }
            catch
            {
                return StatusCode(500);
            }

            Log.Information($"{userId} starts new game");

            
            return Ok(gameId);
        }

        /// <summary>
        /// 게임 클리어 기록
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("clear")]
        public async Task<IActionResult> Clear([FromBody] GameDataDTO gamedata)
        {
            if (gamedata.UserId == null)
            {
                return BadRequest();
            }

            try
            {
                await _gameService.InsertClear(gamedata);
            }
            catch
            {
                return StatusCode(500);
            }

            Log.Information($"{gamedata.UserId} clear the game");

            return Created();
        }

        /// <summary>
        /// 주요 게임 데이터 업데이트
        /// </summary>
        /// <param name="gameDataDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> Put([FromBody] GameDataDTO gameDataDto)
        {
            if(gameDataDto.UserId == null)
            {
                return BadRequest();
            }

            try
            {
                await _gameService.UpdateGameData(gameDataDto);
            }
            catch
            {
                return StatusCode(500);
            }

            Log.Information($"{gameDataDto.UserId} updated data");

            return Ok();
        }

        /// <summary>
        /// 업그레이드 데이터 업데이트
        /// </summary>
        /// <param name="upgradeDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("upgrade")]
        public IActionResult Put([FromBody] UpgradeDTO upgradeDto)
        {
            if (upgradeDto.GameId == null)
            {
                return BadRequest();
            }

            try
            {
                _gameService.UpdateUpgrade(upgradeDto);
            }
            catch
            {
                return StatusCode(500);
            }

            Log.Information($"{upgradeDto.GameId} upgraded ${upgradeDto.UpgradeId}");


            return Ok();
        }

        [HttpGet]
        [Route("secure")]
        public IActionResult AuthTest()
        {
            return Ok();
        }
    }
}
