using System;

namespace BankDataDownloader.Data.Entity.BankTransactions
{
    public class RciTransactionEntity : TransactionEntity, IEntityEqualityComparer<RciTransactionEntity>
    {
        public string ReasonForTransfer { get; set; }
        public string TransferDetail { get; set; }
        public string StatementNumber { get; set; }
        public Func<RciTransactionEntity, bool> Func(RciTransactionEntity otherEntity)
        {
            return
                entity =>
                    entity.AvailabilityDate == otherEntity.AvailabilityDate && /*entity.Text == otherEntity.Text &&*/
                    entity.Amount == otherEntity.Amount && entity.PostingDate == otherEntity.PostingDate &&
                    entity.CurrencyIso == otherEntity.CurrencyIso && entity.ReasonForTransfer == otherEntity.ReasonForTransfer && entity.TransferDetail == otherEntity.TransferDetail && entity.StatementNumber == otherEntity.StatementNumber;
        }
    }
}
