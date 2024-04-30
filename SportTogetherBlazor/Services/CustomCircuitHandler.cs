using Microsoft.AspNetCore.Components.Server.Circuits;

namespace SportTogetherBlazor.Services
{
    public class CustomCircuitHandler : CircuitHandler
    {
        public override Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
        {
            // Logic when the circuit is first opened
            return base.OnCircuitOpenedAsync(circuit, cancellationToken);
        }

        public override Task OnCircuitClosedAsync(Circuit circuit, CancellationToken cancellationToken)
        {
            // Logic when the circuit is closed
            return base.OnCircuitClosedAsync(circuit, cancellationToken);
        }

        public override Task OnConnectionDownAsync(Circuit circuit, CancellationToken cancellationToken)
        {
            // Logic when the connection is lost
            return base.OnConnectionDownAsync(circuit, cancellationToken);
        }

        public override Task OnConnectionUpAsync(Circuit circuit, CancellationToken cancellationToken)
        {
            // Logic when the connection is restored
            return base.OnConnectionUpAsync(circuit, cancellationToken);
        }
    }
}




