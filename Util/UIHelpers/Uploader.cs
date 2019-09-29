using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ASY.Hrefs.Util.UIHelpers
{
    public class Uploader
    {
        private readonly string _root;
        private string _fileName;
        private long _length;
        private string _contentType;
        private long _maxSize = 4 * 1024 * 1024;
        private string _extension;
        private string _fileType;
        private Dictionary<string, string> _extTable = new Dictionary<string, string>();
        private Dictionary<string, string> _imageType = new Dictionary<string, string>();

        public Uploader()
        {
            _extTable.Add("images", "gif,jpg,jpeg,png,bmp,ico");
            _extTable.Add("flashs", "swf,flv");
            _extTable.Add("medias", "swf,flv,mp3,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb");
            _extTable.Add("files", "doc,docx,xls,xlsx,ppt,htm,html,txt,zip,rar,gz,bz2");

            _imageType.Add("image/gif", ".gif");
            _imageType.Add("image/jpeg", ".jpg");
            _imageType.Add("image/png", ".png");
            _imageType.Add("application/x-bmp", ".bmp");
        }

        public Uploader(string root, string filename, long length, string contentType):this()
        {
            _root = root;
            _fileName = filename;
            _length = length;
            _contentType = contentType.ToLowerInvariant();

            if (this.Filter())
            {
                string part = DateTime.Now.ToString("yyyyMMdd");
                string name = Guid.NewGuid().ToString() + _extension;
                
                FileUrl = "/" + _fileType + "/" + part + "/" + name;
                FilePath = Path.Combine(root, _fileType, part);
                this.InitDirectory(FilePath);

                FilePath = Path.Combine(root, _fileType, part, name);
            }
        }

        public bool Filter()
        {
            // 获取文件的后缀
            _extension = Path.GetExtension(_fileName).ToLower();
            if (string.IsNullOrWhiteSpace(_extension))
            {
                if (_imageType.ContainsKey(_contentType))
                {
                    _extension = _imageType[_contentType];
                }
            }

            if(string.IsNullOrWhiteSpace(_extension))
            {
                Message = "文件类型不支持";
            }

            if (_length <= _maxSize)
            {
                Func<string, bool> fun = (string p) =>
                {
                    if (_extTable[p].Split(',').Contains(_extension.Substring(1)))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                };

                IEnumerable<string> exts = _extTable.Keys.Where<string>(fun);
                if (exts.Any())
                {
                    _fileType = exts.FirstOrDefault();
                    Status = true;
                }
                else
                {
                    Message = "不支持的文件类型 ";
                }
            }
            else
            {
                Message = "上传文件超出最大限制";
            }

            return Status;
        }

        public string FileUrl { get; } = string.Empty;

        public string FilePath { get; } = string.Empty;

        public string Message { get; private set; } = string.Empty;

        public bool Status { get; private set; } = false;

        private string InitDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                return path;
            }
            else
            {
                return Directory.CreateDirectory(path).FullName;
            }
        }
    }
}