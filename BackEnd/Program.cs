using Microsoft.EntityFrameworkCore;
using OurNovel.Data;
using OurNovel.Repositories;
using OurNovel.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// ========================================
// 1️⃣ 配置数据库连接（Oracle + EF Core）
// ========================================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));

// ========================================
// 2️⃣ 注册仓储和服务依赖注入
// ========================================

// 注册泛型仓储
builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
// 注册泛型基础服务
builder.Services.AddScoped(typeof(BaseService<,>));
// 注册具体业务服务
builder.Services.AddScoped<ReaderService>();

// ========================================
// 3️⃣ 添加控制器服务
// ========================================
builder.Services.AddControllers();

// ========================================
// 4️⃣ 配置 Swagger（带 XML 注释文档）
// ========================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // 读取 XML 注释文档
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});

// ========================================
// 5️⃣ 构建应用
// ========================================
var app = builder.Build();

// ========================================
// 6️⃣ 配置中间件管道
// ========================================

// 开发环境启用 Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HTTPS 强制跳转
app.UseHttpsRedirection();

// 授权中间件（如果后续启用认证）
app.UseAuthorization();

// 映射控制器路由
app.MapControllers();

// 启动应用
app.Run();
