using Microsoft.AspNetCore.SignalR;

public class CuentaHub : Hub
{
    public async Task EnviarMensajeSaldoDepositado(string mensaje)
    {
        await Clients.All.SendAsync("RecibirSaldoDepositado", mensaje);
    }

    public async Task EnviarMensajeSaldoTransferido(string mensaje)
    {
        await Clients.All.SendAsync("RecibirSaldoTransferido", mensaje);
    }

}