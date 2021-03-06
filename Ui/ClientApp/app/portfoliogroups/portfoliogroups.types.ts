﻿import { PortfolioPositionModel } from "../account/transaction.types";

export class PortfolioGroupModel {
    id: number;
    name: string;
    assignedIsins: Array<string>;
    lowerThresholdPercentage: number;
    targetPercentage: number;
    upperThresholdPercentage: number;
    includeInCalculations: boolean;
    positions: Array<PortfolioPositionModel>;
}