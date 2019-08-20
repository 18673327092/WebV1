<%@ WebHandler Language="c#" Class="ImgCropper_WebHandler" Debug="true" %>

using System;
using System.Web;
using System.Drawing;
using System.IO;

public class ImgCropper_WebHandler : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        string Pic = Convert.ToString(context.Request["p"]);
        int PointX = Convert.ToInt32(context.Request["x"]);
        int PointY = Convert.ToInt32(context.Request["y"]);
        int CutWidth = Convert.ToInt32(context.Request["w"]);
        int CutHeight = Convert.ToInt32(context.Request["h"]);
        int PicWidth = Convert.ToInt32(context.Request["pw"]);
        int PicHeight = Convert.ToInt32(context.Request["ph"]);

        context.Response.ContentType = "image/jpeg";
        ResetImg(System.Web.HttpContext.Current.Server.MapPath(Pic), PicWidth, PicHeight, PointX, PointY, CutWidth, CutHeight).WriteTo(context.Response.OutputStream);
    }
    
    public MemoryStream ResetImg(string ImgFile, int PicWidth, int PicHeight, int PointX, int PointY, int CutWidth, int CutHeight)
    {
        Image imgPhoto = Image.FromFile(ImgFile);
        Bitmap bmPhoto = new Bitmap(CutWidth, CutHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

        Graphics gbmPhoto = Graphics.FromImage(bmPhoto);
        gbmPhoto.DrawImage(imgPhoto, new Rectangle(0, 0, CutWidth, CutHeight), PointX * imgPhoto.Width / PicWidth, PointY * imgPhoto.Height / PicHeight, CutWidth * imgPhoto.Width / PicWidth, CutHeight * imgPhoto.Height / PicHeight, GraphicsUnit.Pixel);

        MemoryStream ms2 = new MemoryStream();
        bmPhoto.Save(ms2, System.Drawing.Imaging.ImageFormat.Jpeg);

        imgPhoto.Dispose();
        gbmPhoto.Dispose();
        bmPhoto.Dispose();

        return ms2;
    }


    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}