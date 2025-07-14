using Microsoft.EntityFrameworkCore;
using OurNovel.Data;
using OurNovel.Repositories;
using OurNovel.Services;
using OurNovel.Services.FileStorage.Interfaces;
using OurNovel.Services.FileStorage;
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
builder.Services.AddScoped<IChapterRepository, ChapterRepository>();
builder.Services.AddScoped<ChapterService>();
builder.Services.AddScoped<NovelService>();
// 注册文件存储配置
builder.Services.Configure<FileStorageOptions>(
    builder.Configuration.GetSection("FileStorage"));
// 注册文件存储服务
builder.Services.AddScoped<IFileStorageService, LocalFileStorageService>();

// ========================================
// 3️⃣ 添加控制器服务
// ========================================
builder.Services.AddControllers();

// ========================================
// 4️⃣ 配置 CORS（新增部分）
// ========================================
var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()
    ?? throw new Exception("CORS 配置未找到！请检查 appsettings.json");

builder.Services.AddCors(options =>
{
    options.AddPolicy("SpecificOrigins", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyMethod()
              .AllowAnyHeader();

        // 如需凭证（如 Cookies），取消注释：
        // .AllowCredentials(); 
    });
});

// ========================================
// 5  配置 Swagger（带 XML 注释文档）
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
// 6   构建应用
// ========================================
var app = builder.Build();

// ========================================
// 7  配置中间件管道
// ========================================

// 开发环境启用 Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HTTPS 强制跳转
app.UseHttpsRedirection();

app.UseRouting();

// 启用 CORS 中间件（必须在 UseRouting 之后，UseEndpoints 之前）
app.UseCors("SpecificOrigins");

// 授权中间件（如果后续启用认证）
app.UseAuthorization();

// 映射控制器路由
app.MapControllers();

// 启动应用
app.Run();
