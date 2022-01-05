using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using BeaHelper.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using QRCoder;


namespace SyrusVoluntariado.Controllers
{
    public class QRCodeController : Controller
    {
        [HttpGet]
        public IActionResult Index(string qrTexto)
        {
            string nomearquivo = Regex.Replace(qrTexto, ".*visualizar/", "");
            nomearquivo = "evento" + nomearquivo;
            DeleteArquivo(nomearquivo);
            QRCodeGenerator qrGerador = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGerador.CreateQrCode(qrTexto,QRCodeGenerator.ECCLevel.Q);

            qrCodeData.SaveRawData("wwwroot/qrcode/arquivo-" + nomearquivo + ".qrr",
                   QRCodeData.Compression.Uncompressed);
            QRCodeData qrCodeData1 = new QRCodeData("wwwroot/qrcode/arquivo-" + nomearquivo + ".qrr", 
                QRCodeData.Compression.Uncompressed);

            QRCode qrCode = new QRCode(qrCodeData1);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            ViewBag.QrCodeByte = BitmapToBytes(qrCodeImage);
            return View(BitmapToBytes(qrCodeImage));
        }

        private static Byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }

        public void DeleteArquivo(string arquivo)
        {
            //arquivo = "wwwroot/qrr/arquivo-1254.qrr";
            ValidaArquivo.DeletaArquivo(arquivo);
        }

    }
}