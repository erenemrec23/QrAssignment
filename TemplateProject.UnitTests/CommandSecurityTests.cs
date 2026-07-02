using QrAssignment.Application.Features.Tenants.Commands.Create;
using QrAssignment.Application.Interfaces;
using System.Reflection;
using Xunit;
// Kendi projendeki namespace'leri buraya eklemelisin
// using QrAssignment.Application.Features.Tenants.Commands.Create;
// using QrAssignment.Domain.Shared;

namespace QrAssignment.Tests.Architecture
{
    public class CommandSecurityTests
    {
        [Fact]
        public void All_Commands_Must_Implement_ISecuredRequest_Or_INotSecuredRequest()
        {
            // 1. Arrange: Application katmanındaki tüm tipleri bul
            // Herhangi bir Command (örn. CreateTenantCommand) üzerinden Assembly'i yakalıyoruz.
            var applicationAssembly = typeof(CreateTenantCommand).Assembly;

            // Command sınıflarını filtreliyoruz (Class olmalı, Abstract olmamalı ve adı Command ile bitmeli)
            var commandTypes = applicationAssembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .Where(t => t.Name.EndsWith("Command"))
                .ToList();

            var failingCommands = new List<string>();

            // 2. Act: Her bir Command için kuralı kontrol et
            foreach (var type in commandTypes)
            {
                // Tipin aradığımız interfaceleri implemente edip etmediğini kontrol ediyoruz
                bool isSecured = typeof(ISecuredRequest).IsAssignableFrom(type);
                bool isNotSecured = typeof(INotSecuredRequest).IsAssignableFrom(type);

                // Eğer ikisi de yoksa, bu sınıf kuralı ihlal etmiştir
                if (!isSecured && !isNotSecured)
                {
                    failingCommands.Add(type.Name);
                }

                // İsteğe bağlı ekstra kural: Bir sınıf aynı anda İKİSİNİ BİRDEN implemente edemez
                if (isSecured && isNotSecured)
                {
                    failingCommands.Add($"{type.Name} (HATA: Hem Secured Hem NotSecured implemente edilmiş!)");
                }
            }

            // 3. Assert: Hata veren sınıfların listesi boş olmalıdır.
            // Eğer boş değilse, Assert patlar ve string.Join ile hangi sınıfların kuralı ihlal ettiğini Test Runner ekranında sana gösterir.
            Assert.True(failingCommands.Count == 0,
                $"Güvenlik arayüzü eksik olan Command'ler bulundu:\n" + string.Join("\n", failingCommands));
        }
    }
}