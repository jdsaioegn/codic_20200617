using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ClassLibrary
{
    /// <summary>
    /// 接続設定画面設定用機能群
    /// </summary>
    public class Class1
    {
        #region 環境設定ファイルオブジェクト
        /// <summary>
        /// 環境設定ファイルオブジェクト
        /// </summary>
        class SettingProfile
        {
            /// <summary>
            /// 接続方式
            /// </summary>
            public string ConnectionType { get; set; }

            /// <summary>
            /// ログイン情報保存機能フラグ
            /// </summary>
            public bool LoginInfoFlg { get; set; }

            /// <summary>
            /// 接続先ホストマスタリスト
            /// </summary>
            public List<string> HostNameMast { get; set; }

            /// <summary>
            /// 接続先データベースマスタリスト
            /// </summary>
            public List<string> DatabaseNameMast { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public SettingProfile()
            {
                // 環境設定ファイル読込
                XElement xml = XElement.Load(@".\環境設定.xml");

                // 環境設定ファイル読込情報解析処理
                foreach (XElement node in from item in xml.Elements("Config") select item)
                {
                    // 接続方式設定値取得
                    ConnectionType = node.Element("ConnectionType").Value;

                    // ログイン情報保存機能フラグ
                    LoginInfoFlg = node.Element("LoginInfoFlg").Value.ToString() == "ON" ? true : false;

                    // 接続先ホストマスタリスト設定用
                    IEnumerable<XElement> host = from item in node.Elements("HostMast") select item;

                    // 接続先ホストマスタリスト設定
                    HostNameMast = host.Elements("Value").Select(p => p.Value).ToList();

                    // 接続先データベースマスタリスト設定用
                    IEnumerable<XElement> database = from item in node.Elements("DatabaseMast") select item;

                    // 接続先データベースマスタリスト設定
                    DatabaseNameMast = database.Elements("Value").Select(p => p.Value).ToList();
                }
            }
        }
        #endregion

        #region 詳細設定ファイルオブジェクト
        /// <summary>
        /// 詳細設定ファイルオブジェクト
        /// </summary>
        class LoginProfile
        {
            /// <summary>
            /// 最終ログインユーザ
            /// </summary>
            public string LoginUser { get; set; }

            /// <summary>
            /// 最終ログインパスワード
            /// </summary>
            public string LoginPassword { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public LoginProfile()
            {
                // 詳細設定ファイル読込
                XElement xml = XElement.Load(@".\詳細設定.xml");

                // 詳細設定ファイル読込情報解析処理
                foreach (XElement node in from item in xml.Elements("Config") select item)
                {
                    // 最終ログインユーザ設定
                    LoginUser = node.Element("LoginUser").Value;

                    // 最終ログインパスワード設定
                    LoginPassword = node.Element("LoginPassword").Value;
                }
            }
        }
        #endregion

        #region 環境設定ファイル更新処理
        /// <summary>
        /// 環境設定ファイル更新処理
        /// </summary>
        /// <param name="node">更新要素</param>
        /// <param name="value">更新値</param>
        private void SaveSettingProfile(string node, string value)
        {
            // 環境設定ファイル読込
            XElement xml = XElement.Load(@".\環境設定.xml");

            // 環境設定ファイル読込情報解析処理
            foreach (XElement item in from item in xml.Elements("Config") select item)
            {
                // 環境設定ファイル更新
                item.Element(node).Value = value;

                // 環境設定ファイル保存
                xml.Save(@".\環境設定.xml");
            }
        }
        #endregion

        #region 詳細設定ファイル更新処理
        /// <summary>
        /// 詳細設定ファイル更新処理
        /// </summary>
        /// <param name="user">設定値（ログインユーザ）</param>
        /// <param name="password">設定値（ログインパスワード）</param>
        private void SaveLoginProfile(string user, string password)
        {
            // 詳細設定ファイル指定
            FileInfo file = new FileInfo(@".\詳細設定.xml");

            // 詳細設定ファイル隠しファイル属性解除
            file.Attributes &= ~FileAttributes.Hidden;

            // 詳細設定ファイル読み取り専用ファイル属性削除
            file.Attributes &= ~FileAttributes.ReadOnly;

            // 環境設定ファイル読込
            XElement xml = XElement.Load(@".\詳細設定.xml");

            // 環境設定ファイル読込情報解析処理
            foreach (XElement item in from item in xml.Elements("Config") select item)
            {
                // 最終ログインユーザを更新
                item.Element("LoginUser").Value = user;

                // 最終ログインパスワードを更新
                item.Element("LoginPassword").Value = password;

                // 詳細設定ファイル保存
                xml.Save(@".\詳細設定.xml");
            }

            // 詳細設定ファイル読み取り専用ファイル属性付与
            file.Attributes |= FileAttributes.ReadOnly;

            // 詳細設定ファイル隠しファイル属性付与
            file.Attributes |= FileAttributes.Hidden;
        }
        #endregion
    }
}
