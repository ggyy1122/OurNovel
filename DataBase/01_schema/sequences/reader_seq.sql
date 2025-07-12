-- 创建读者表的主键序列
CREATE SEQUENCE "READER_SEQ" 
START WITH 1              -- 从1开始
INCREMENT BY 1            -- 每次递增1
NOCACHE                  -- 禁用缓存保证连续性
NOCYCLE;                 -- 不循环使用