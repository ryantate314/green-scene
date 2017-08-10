using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GreenScene.Dto;

namespace GreenScene.Web.Controllers
{
   [Route("api/[controller]")]
   public class ScenesController : Controller
   {

      private List<SceneDto> _scenes;

      public ScenesController()
      {
         _scenes = new List<SceneDto>();
         _scenes.Add(new SceneDto()
         {
            Key = 1,
            Title = "Roach Motel",
            Description = "Lorem ipsum dolor sit amet."
         });
         _scenes.Add(new SceneDto()
         {
            Key = 2,
            Title = "Nice Hotel",
            Description = "Lorem ipsum dolor sit amet."
         });
      }

      [HttpGet]
      public IEnumerable<SceneDto> Get()
      {
         return _scenes;
      }

      [HttpGet("{id}")]
      public SceneDto Get(int id)
      {
         System.Threading.Thread.Sleep(500);
         return _scenes.FirstOrDefault(s => s.Key == id);
      }
   }
}