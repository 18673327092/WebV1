using Base.IService;
using Base.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Utility;
using Utility.Container;
using Utility.ResultModel;

namespace Web.Controllers
{
    public class ImageController : ApiController
    {
        public IImagesService imagesService { get; set; }
        public ImageController()
        {
            //imagesService = ObjectContainer.Current.Resolve<IImagesService>();
        }

        public async Task<HttpResponseMessage> Get(int id, int type = 0)
        {
            var res = await imagesService.GetAsync(id);
            var src = res.Data.Src;
            if (type == 1)
            {
                src = res.Data.OriginalSrc;
            }
            else if (type == 2)
            {
                src = res.Data.ThumbnailSrc;
            }
            try
            {
                if (string.IsNullOrEmpty(src)) src = res.Data.Src;
                var imgPath = System.Web.Hosting.HostingEnvironment.MapPath("~/" + src);
                //从图片中读取byte
                var imgByte = File.ReadAllBytes(imgPath);
                //从图片中读取流
                var imgStream = new MemoryStream(File.ReadAllBytes(imgPath));
                var resp = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StreamContent(imgStream)
                    //或者
                    //Content = new ByteArrayContent(imgByte)
                };
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");
                return resp;
            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

        }


    }
}
