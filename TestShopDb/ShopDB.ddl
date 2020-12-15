CREATE DATABASE ShopDB
USE ShopDB
CREATE TABLE [dbo].[Goods] (
    [Id]       INT            DEFAULT (NEXT VALUE FOR [dbo].[Goods_Id_Sequence]) NOT NULL,
    [Name]     NVARCHAR (MAX) NULL,
    [ShopId]   NVARCHAR (50)  NOT NULL,
    [IdInShop] INT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);