using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace beipin
{
    class SqlHelper
    {
        // SQL Server 连接字符串（可从INI配置读取，这里先固定，后续优化）
        private readonly string _connStr;

        /// <summary>
        /// 构造函数：传入连接字符串
        /// </summary>
        /// <param name="connStr">SQL Server 连接字符串</param>
        public SqlHelper(string connStr)
        {
            _connStr = connStr;
        }

        /// <summary>
        /// 插入解析表（每次数据采集时执行，关联唯一processNo）
        /// </summary>
        /// <param name="processNo">10位唯一工序号（时间戳生成）</param>
        /// <returns>是否插入成功</returns>
        public bool InsertProcessParseTable(string processNo)
        {
                string sql = @"
                    -- 插入工位2状态映射（关联当前processNo）
                    INSERT INTO SHProcessPropertyParse(process_no, field_name, field_name_cn, data_type)
                    VALUES(@process_no, 'data001', '二维码等级', 'varchar');

                    -- 插入工位3状态映射
                    --INSERT INTO SHProcessPropertyParse(process_no, field_name, field_name_cn, data_type)
                    --VALUES(@process_no, 'data002', '工位3状态', 'varchar');

                    -- 插入工位4状态映射
                    --INSERT INTO SHProcessPropertyParse(process_no, field_name, field_name_cn, data_type)
                    --VALUES(@process_no, 'data003', '工位4状态', 'varchar');

                    -- 插入相机判断状态映射
                    --INSERT INTO SHProcessPropertyParse(process_no, field_name, field_name_cn, data_type)
                    --VALUES(@process_no, 'data004', '相机判断状态', 'int');";

                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // 绑定当前唯一processNo
                        cmd.Parameters.Add("@process_no", SqlDbType.VarChar, 10).Value = processNo;
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
        }

        /// <summary>
        /// 插入存储表（关联当前processNo）
        /// </summary>
        public bool InsertProcessProperty(
            string processNo,       // 唯一工序号（与解析表关联）
            string barNo,           // 条码（PLC扫描二维码）
            string station2Status,  // 工位2状态（data001）
            string station3Status,  // 工位3状态（data002）
            string station4Status,  // 工位4状态（data003）
            ushort cameraStatus,     // 相机判断状态（data004）,
            string okflag,
            string qrCodeLevel,
            string vouNo = "",      // 派工单（无则空）
            string userId = "PLC",  // 用户名（默认PLC）
            string eqptLocId = ""   // 设备子工位号（无则空）
        )
        {
            try
            {
                string sql = @"INSERT INTO SHProcessProperty(
                                bar_no, process_no, do_time, start_time, vou_no, 
                                item_no, ok_flag, ng_msg, user_id, flag, 
                                eqpt_loc_id, major_state, second_state, aux_state,
                                data001
                            ) VALUES(
                                @bar_no, @process_no, GETDATE(), GETDATE(), @vou_no,
                                '', @ok_flag, '', @user_id, 1,
                                @eqpt_loc_id, 0, 0, 0,
                                @data001
                            )";

                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // 核心：绑定与解析表一致的processNo
                        cmd.Parameters.Add("@process_no", SqlDbType.VarChar, 10).Value = processNo;
                        // 基础字段
                        cmd.Parameters.Add("@bar_no", SqlDbType.VarChar, 200).Value = barNo ?? "";
                        cmd.Parameters.Add("@vou_no", SqlDbType.VarChar, 20).Value = vouNo;
                        cmd.Parameters.Add("@user_id", SqlDbType.VarChar, 8).Value = userId;
                        cmd.Parameters.Add("@eqpt_loc_id", SqlDbType.VarChar, 20).Value = eqptLocId;
                        // PLC数据映射
                        cmd.Parameters.Add("@data001", SqlDbType.VarChar, 50).Value = qrCodeLevel ?? "";
                        cmd.Parameters.Add("@ok_flag", SqlDbType.VarChar, 8).Value = okflag;

                        /* cmd.Parameters.Add("@data002", SqlDbType.VarChar, 50).Value = okflag ?? "";
                         cmd.Parameters.Add("@data003", SqlDbType.VarChar, 50).Value = station4Status ?? "";
                         cmd.Parameters.Add("@data004", SqlDbType.Int).Value = cameraStatus;
                         cmd.Parameters.Add("@data005", SqlDbType.VarChar, 50).Value = qrCodeLevel ?? "";*/

                        int rows = cmd.ExecuteNonQuery();
                        return rows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"存储表插入失败：{ex.Message}", "SQL错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// 写入文件信息到SHProcessFile表（适配最新字段结构）
        /// </summary>
        /// <param name="bar_no">主条码（PLC扫描的二维码）</param>
        /// <param name="process_no">工序号（10位唯一序号）</param>
        /// <param name="file_type">文件类型（Img/CSV/WVA）</param>
        /// <param name="name">文件名称（含扩展名）</param>
        /// <param name="do_time">文件产生时间（文件创建时间）</param>
        /// <param name="ok_flag">文件判定结果（OK/NG，默认OK）</param>
        /// <param name="ng_msg">不良原因（无则空）</param>
        /// <param name="path">FTP完整路径（三花内网IP+文件路径）</param>
        /// <param name="flag">调度标记（0未调度/1完成/4不存在，默认0）</param>
        /// <param name="sync_time">调度时间（未调度则为null）</param>
        /// <param name="sync_msg">调度异常信息（无则空）</param>
        /// <returns>是否写入成功</returns>
        public bool InsertProcessFileInfo(
            string bar_no,
            string process_no,
            string file_type,
            string name,
            DateTime do_time,
            string ok_flag,
            string ng_msg = "",
            string path = "",
            int flag = 0
        )
        {
            try
            {
                // SQL语句匹配新表所有字段（id自增无需传入）
                string sql = @"INSERT INTO SHProcessFile(
                                bar_no, process_no, file_type, name, do_time,
                                ok_flag, ng_msg, path, flag
                            ) VALUES(
                                @bar_no, @process_no, @file_type, @name, @do_time,
                                @ok_flag, @ng_msg, @path, @flag
                            )";

                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // 绑定所有字段参数（严格匹配表结构）
                        cmd.Parameters.Add("@bar_no", SqlDbType.VarChar, 200).Value = bar_no ?? "";
                        cmd.Parameters.Add("@process_no", SqlDbType.VarChar, 10).Value = process_no ?? "";
                        cmd.Parameters.Add("@file_type", SqlDbType.VarChar, 20).Value = file_type ?? "";
                        cmd.Parameters.Add("@name", SqlDbType.VarChar, 200).Value = name ?? "";
                        cmd.Parameters.Add("@do_time", SqlDbType.DateTime).Value = do_time;
                        cmd.Parameters.Add("@ok_flag", SqlDbType.VarChar, 20).Value = ok_flag ?? "";
                        cmd.Parameters.Add("@ng_msg", SqlDbType.VarChar, 200).Value = ng_msg ?? "";
                        cmd.Parameters.Add("@path", SqlDbType.VarChar, 500).Value = path ?? "";
                        cmd.Parameters.Add("@flag", SqlDbType.Int).Value = flag;
                        // 可空字段处理（调度时间未设置则传DBNull）
                       // cmd.Parameters.Add("@sync_time", SqlDbType.DateTime).Value = sync_time;
                       // cmd.Parameters.Add("@sync_msg", SqlDbType.VarChar, 200).Value = sync_msg ?? "";

                        int rows = cmd.ExecuteNonQuery();
                        return rows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"写入SHProcessFile表失败：{ex.Message}", "SQL错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
