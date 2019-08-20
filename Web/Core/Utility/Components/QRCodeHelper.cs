using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using ThoughtWorks.QRCode.Codec;

namespace Utility.Components
{
    public class QRCodeHelper
    {
        private QRCodeHelper()
        {
        }

        private static QRCodeHelper _single = new QRCodeHelper();

        public static QRCodeHelper Single
        {
            get { return _single; }
        }

        public Bitmap Create(string content)
        {
            //创建二维码生成类
            var qrcode = new QRCodeEncoder();
            //设置编码模式
            qrcode.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            //设置编码测量度
            qrcode.QRCodeScale = 4;
            //设置编码版本
            qrcode.QRCodeVersion = 8;
            //设置编码错误纠正  
            qrcode.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;

            var image = qrcode.Encode(content);
            return image;
        }
    }
}
