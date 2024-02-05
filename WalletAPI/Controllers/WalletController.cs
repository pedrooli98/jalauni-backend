
using Microsoft.AspNetCore.Mvc;
using WalletAPI.Models;
using System;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class WalletController : ControllerBase
{
    private readonly WalletDbContext _context;

    public WalletController(WalletDbContext context)
    {
        _context = context;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateWallet([FromBody] Wallet wallet)
    {
        try
        {
            _context.Wallets.Add(wallet);
            await _context.SaveChangesAsync();
            return Ok("Wallet created successfully");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    [HttpGet("list")]
    public IActionResult ListWallets()
    {
        var wallets = _context.Wallets.ToList();
        return Ok(wallets);
    }

    [HttpPost("{walletId}/add-currency")]
    public async Task<IActionResult> AddCurrencyToWallet(int walletId, [FromBody] double amount)
    {
        var wallet = await _context.Wallets.FindAsync(walletId);
    
        if (wallet == null)
        {
            return NotFound("Wallet not found");
        }
    
        wallet.Balance += amount;
        _context.Wallets.Update(wallet);
        await _context.SaveChangesAsync();
    
        return Ok(wallet.Balance);
    }
}