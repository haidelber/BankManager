﻿using System;

namespace BankManager.Data.Entity.BankTransactions
{
    public class RciTransactionEntity : TransactionEntity, IEntityEqualityComparer<RciTransactionEntity>
    {
        public string ReasonForTransfer { get; set; }
        [Obsolete("Was available in old export")]
        public string TransferDetail { get; set; }
        [Obsolete("Was available in old export")]
        public string StatementNumber { get; set; }
        public Func<RciTransactionEntity, bool> Func(RciTransactionEntity otherEntity)
        {
            return
                entity =>
                    entity.AvailabilityDate == otherEntity.AvailabilityDate && entity.Text == otherEntity.Text &&
                    entity.Amount == otherEntity.Amount && entity.PostingDate == otherEntity.PostingDate &&
                    entity.CurrencyIso == otherEntity.CurrencyIso && entity.ReasonForTransfer == otherEntity.ReasonForTransfer;
        }
    }
}
