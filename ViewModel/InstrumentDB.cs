using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;

namespace ViewModel
{
    public class InstrumentDB : BaseDB
    {
        protected override BaseEntity NewEntity()
        {
            return new Instrument();
        }

        protected override void CreateModel(BaseEntity entity)
        {
            Instrument instrument = entity as Instrument;
            instrument.Id = (int)this.reader["id"];
            instrument.Name = this.reader["insName"].ToString();
        }

        public override string CreateInsertSQL(BaseEntity entity)
        {
            throw new NotImplementedException("Not inserting new instruments currently");
        }

        public override string CreateUpdateSQL(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

        public override string CreateDeleteSQL(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

        public InstrumentList SelectAll()
        {
            this.command.CommandText = "SELECT * FROM tblInstruments";
            return new InstrumentList(base.Select());
        }

        public InstrumentList GetUserInstruments(int userId)
        {
            string sql = $"SELECT tblInstruments.* FROM tblInstruments INNER JOIN tblInstList ON tblInstruments.id = tblInstList.instId WHERE tblInstList.userid = {userId}";
            this.command.CommandText = sql;
            return new InstrumentList(base.Select());
        }

        public void AddUserInstrument(int userId, int instrumentId)
        {
            // Check if already exists
            string checkSql = $"SELECT COUNT(*) FROM tblInstList WHERE userid = {userId} AND instId = {instrumentId}";
            
            try
            {
                if (this.connection.State != ConnectionState.Open)
                    this.connection.Open();

                OleDbCommand checkCmd = new OleDbCommand(checkSql, this.connection);
                int count = (int)checkCmd.ExecuteScalar();
                
                if (count > 0)
                    return; // Already exists, don't add duplicate

                string sql = $"INSERT INTO tblInstList (userid, instId) VALUES ({userId}, {instrumentId})";
                OleDbCommand cmd = new OleDbCommand(sql, this.connection);
                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error adding user instrument: {ex.Message}");
                throw;
            }
            finally
            {
                if (this.connection.State == ConnectionState.Open)
                    this.connection.Close();
            }
        }

        public void RemoveUserInstrument(int userId, int instrumentId)
        {
            string sql = $"DELETE FROM tblInstList WHERE userid = {userId} AND instId = {instrumentId}";
            
            try
            {
                if (this.connection.State != ConnectionState.Open)
                    this.connection.Open();

                OleDbCommand cmd = new OleDbCommand(sql, this.connection);
                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error removing user instrument: {ex.Message}");
                throw;
            }
            finally
            {
                if (this.connection.State == ConnectionState.Open)
                    this.connection.Close();
            }
        }
    }
}
