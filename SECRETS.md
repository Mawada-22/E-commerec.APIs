# Configuration secrets

`appsettings.json` is committed to a public repo, so every secret in it is left
**blank on purpose**. Fill them in per-environment using one of the two methods below.

| Setting | What it is |
|---|---|
| `JwtOptions:SecretKey` | Signing key for login tokens. Anyone with it can forge logins. |
| `StripeSettings:SecretKey` | Stripe **secret** key (`sk_test_…` / `sk_live_…`). Can move money. |
| `StripeSettings:WebhookSecret` | Stripe webhook signing secret (`whsec_…`). Verifies callbacks are genuine. |
| `SeedUsers:Password` | Password given to the seeded Admin / SuperAdmin accounts. |

`StripeSettings:PublishableKey` (`pk_test_…`) is **not** secret — it is designed to be
public and is safe to commit.

## Local development (user-secrets)

Stored outside the repo, per-machine:

```
cd E-commerce.Apis
dotnet user-secrets set "JwtOptions:SecretKey" "<a long random string>"
dotnet user-secrets set "StripeSettings:SecretKey" "sk_test_..."
dotnet user-secrets set "StripeSettings:WebhookSecret" "whsec_..."
dotnet user-secrets set "SeedUsers:Password" "<seed admin password>"
dotnet user-secrets list
```

## Production (IIS / App Service)

Set them as **environment variables**, using `__` (double underscore) in place of `:`:

```
JwtOptions__SecretKey
StripeSettings__SecretKey
StripeSettings__WebhookSecret
SeedUsers__Password
```

On IIS this goes in the app pool's environment, or via `web.config`
`<environmentVariables>`; on Azure App Service it is Configuration → Application settings.

## Getting the Stripe values

- **Secret key**: Stripe Dashboard → Developers → API keys (test mode).
- **Webhook secret**: printed by `stripe listen --forward-to https://localhost:7100/payments/webhook`
  as `whsec_...`, or shown on the endpoint's page in the Dashboard for production.
  Store **only** the `whsec_...` value — no surrounding text.
