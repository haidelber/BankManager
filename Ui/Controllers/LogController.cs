﻿using System;
using BankDataDownloader.Core.Service;
using BankDataDownloader.Ui.Model.DownloadHandler;
using Microsoft.AspNetCore.Mvc;

namespace BankManager.Ui.Controllers
{
    [Route("api/[controller]")]
    public class LogController : ApiController
    {
        public ILogService LogService { get; }

        public LogController(ILogService logService)
        {
            LogService = logService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Json(LogService.GetLogFilePaths());
        }

        [HttpGet("[action]")]
        public IActionResult GetContent(string path)
        {
            return Json(LogService.GetLogs(path));
        }
    }
}